using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.LawfulMod.CivicsImpExp;
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
        public string StoreSection(int lawId, int sectionIndex)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                var section = law.Sections[sectionIndex];
                var json = JsonConvert.SerializeObject(section, Formatting.Indented, new CivicsJsonConverter());
                var sectionDoc = new BsonDocument();
                sectionDoc["title"] = section.Title;
                sectionDoc["description"] = TextUtils.StripTags(section.Description());
                sectionDoc["userDescription"] = section.UserDescription;
                sectionDoc["json"] = json;
                var id = LawfulPlugin.Obj.Db?.GetCollection("sections").Insert(sectionDoc);
                return id.ToString();
            }
            else
            {
                return "";
            }
        }

        public record StoredSectionDto(string Id, string Title, string Description, string UserDescription);

        [HttpGet]
        [AllowAnonymous]
        public StoredSectionDto[] GetStoredSections()
        {
            var sections = LawfulPlugin.Obj.Db?.GetCollection("sections").FindAll();
            return sections.Select(s => new StoredSectionDto(s["_id"].ToString(), s["title"], s["description"], s["userDescription"])).ToArray();
        }
    }
}
