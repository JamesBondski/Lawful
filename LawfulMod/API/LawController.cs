using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Civics.Misc;
using Eco.Mods.LawfulMod.CivicsImpExp;
using LawfulMod.Data;
using LawfulMod.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/law")]
    public class LawController : BaseController
    {
        public record LawDto(int Id, string Title, string Description, string Creator, String State, string Settlement, string? HostObject);

        [HttpGet]
        [AllowAnonymous]
        public LawDto[] GetLaws()
        {
            var laws = Registrars.Get<Law>().Where(l => l.Active());
            return laws.Select(l => GetDto(l)).ToArray();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetLaw(int id)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                return Ok(GetDto(law));
            }
            else
            {
                return NotFound();
            }
        }

        private LawDto GetDto(Law law)
        {
            return new LawDto(law.Id, law.Name, law.UserDescription, law.Creator.Name, law.State.ToString(), law.Settlement.Name, law.HostObject.Object?.Name);
        }

        public record SectionDto(int LawId, int Index, string Title, string Description, string UserDescription, bool CanStore);

        [HttpGet("{lawId}/sections")]
        public SectionDto[] GetSection(int lawId)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                return law.Sections.Select((s, index) => new SectionDto(law.Id, index, s.Title, TextUtils.StripTags(s.Description()), s.UserDescription, this.CanStore(lawId, index))).ToArray();
            }
            else
            {
                return new SectionDto[0];
            }
        }

        [HttpPost("{lawId}/sections/import/{sectionId}")]
        public IActionResult ImportSection(int lawId, int sectionId)
        {
            var section = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").FindById(sectionId);
            if (section == null)
            {
                return NotFound();
            }

            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law == null)
            {
                return NotFound();
            }

            if (!this.CanImport(law, section))
            {
                return StatusCode(403);
            }

            var bundle = CivicBundle.LoadFromText(section.JSON);
            var sectionObj = new ImportContext().DeserialiseGenericObject(bundle.Civics.First().Data, typeof(LawSection));
            law.Sections.Add((LawSection)sectionObj);
            return Ok("");
        }
    }
}
