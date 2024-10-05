using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.LawfulMod.CivicsImpExp;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LawfulMod.API
{
    [Route("api/v1/lawful")]
    public class CivicsController : Controller
    {
        [HttpPost("store")]
        [AllowAnonymous]
        public string StoreSection(int lawId, int sectionIndex)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                var section = law.Sections[sectionIndex];
                var json = JsonConvert.SerializeObject(section, Formatting.Indented, new CivicsJsonConverter());
                var sectionDoc = new BsonDocument();
                sectionDoc["lawId"] = lawId;
                sectionDoc["sectionIndex"] = sectionIndex;
                sectionDoc["json"] = json;
                var id = LawfulPlugin.Obj.Db?.GetCollection("sections").Insert(sectionDoc);
                return id.ToString();
            }
            else
            {
                return "";
            }
        }

        [HttpGet("json")]
        [AllowAnonymous]
        public string SerializeLaw(int Id)
        {

            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == Id);
            if (law != null)
            {
                var json = JsonConvert.SerializeObject(law, Formatting.Indented, new CivicsJsonConverter());

                var lawDoc = new BsonDocument();
                lawDoc["id"] = law.Id;
                lawDoc["json"] = json;
                LawfulPlugin.Obj.Db?.GetCollection("laws").Insert(lawDoc);
                return json;
            }
            else
            {
                return "";
            }
        }


        [HttpGet("sectionjson")]
        [AllowAnonymous]
        public string GetSectionJSON(int id)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                return JsonConvert.SerializeObject(law.Sections.First(), Formatting.Indented, new CivicsJsonConverter());
            }
            else
            {
                return "";
            }
        }
    }
}
