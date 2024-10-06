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
    public class StoreController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public int StoreSection(int lawId, int sectionIndex)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
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
                return id.AsInt32;
            }
            else
            {
                return -1;
            }
        }

        public record StoredSectionDto(int Id, string Title, string Description, string UserDescription);

        [HttpGet]
        [AllowAnonymous]
        public StoredSectionDto[] GetStoredSections()
        {
            var sections = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").FindAll(); // Update to use the POCO class
            return sections.Select(s => new StoredSectionDto(s.Id, s.Title, s.Description, s.UserDescription)).ToArray();
        }
    }
}
