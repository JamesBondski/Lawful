using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Players;
using Eco.Mods.LawfulMod.CivicsImpExp;
using Eco.WebServer.Web.Authentication;
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
        private User? ContextUser => (HttpContext.User.Identity as EcoUserIdentity)?.User;

        [HttpPost]
        public IActionResult StoreSection(int lawId, int sectionIndex)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                if (!AuthorizationHelper.CanStore(ContextUser, lawId, sectionIndex)) // Check if user is authorized to store
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
