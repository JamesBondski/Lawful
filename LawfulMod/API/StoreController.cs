using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.LawfulMod.CivicsImpExp;
using LawfulMod.Data;
using LawfulMod.Util;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/store")]
    public class StoreController : BaseController
    {
        [HttpPost]
        public IActionResult StoreSection(int lawId, int sectionIndex)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                if (!this.CanStore(lawId, sectionIndex)) // Check if user is authorized to store
                {
                    return StatusCode(403); // Return 403 Forbidden if not authorized
                }

                var section = law.Sections[sectionIndex];
                var json = JsonConvert.SerializeObject(section, Formatting.Indented, new CivicsJsonConverter());
                var sectionDoc = new SectionDocument // Use the new POCO class
                {
                    Title = section.Title,
                    Description = TextUtils.StripTags(section.Description()),
                    UserDescription = section.UserDescription,
                    JSON = json
                };
                var id = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").Insert(sectionDoc); // Update to use the POCO class

                if (id == null)
                {
                    return StatusCode(500); // Return 500 Internal Server Error if id is null
                }

                return Ok(id.AsInt32); // Return 200 OK with the section id
            }
            else
            {
                return NotFound(); // Return 404 Not Found if the law is not found
            }
        }

        public record StoredSectionDto(int Id, string Title, string Description, string UserDescription, bool CanImport, bool CanDelete);

        [HttpGet]
        [AllowAnonymous]
        public StoredSectionDto[] GetStoredSections(int selectedLawId = 0)
        {
            var sections = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").FindAll();
            if(sections == null)
            {
                return new StoredSectionDto[0];
            }

            // We need the law to check if the user can import the section. If it is not a draft, it can not be imported to.
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == selectedLawId);
            return sections.Select(s => new StoredSectionDto(s.Id, s.Title, s.Description, s.UserDescription, this.CanImport(law, s), this.CanDelete(s))).ToArray();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStoredSection(int id)
        {
            var sectionCollection = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections");
            var section = sectionCollection?.FindById(id);
            
            //Check for sectionCollection is not necessary here
            if (section == null || sectionCollection == null)
            {
                return NotFound(); // Return 404 Not Found if the section is not found
            }

            if (!this.CanDelete(section))
            {
                return StatusCode(403); // Return 403 Forbidden if not authorized
            }

            sectionCollection.Delete(id); // Delete the section by id
            return NoContent(); // Return 204 No Content on successful deletion
        }

        [HttpGet("{id}/json")]
        public IActionResult GetJson(int id, string filename)
        {
            var section = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").FindById(id);
            if (section == null)
            {
                return NotFound();
            }

            return Ok(section.JSON);
        }
    }
}
