using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.CivicsImpExp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LawfulMod.API
{
    [Route("api/v1/lawful")]
    public class CivicsController : Controller
    {
        public record SectionDto(string Title);

        [HttpGet("section")]
        [AllowAnonymous]
        public SectionDto[] GetSection(int id)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                return law.Sections.Select(s => new SectionDto(s.Title)).ToArray();
            }
            else
            {
                return new SectionDto[0];
            }
        }

        public record LawDto(int Id, string Name);

        [HttpGet("law")]
        [AllowAnonymous]
        public LawDto[] GetLaws()
        {
            var laws = Registrars.Get<Law>().All();
            return laws.Select(l => new LawDto(l.Id, l.Name)).ToArray();
        }

        [HttpGet("json")]
        [AllowAnonymous]
        public string SerializeLaw(int Id)
        {

           var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == Id);
            if (law != null)
            {
                return JsonConvert.SerializeObject(law, Formatting.Indented, new CivicsJsonConverter());
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
