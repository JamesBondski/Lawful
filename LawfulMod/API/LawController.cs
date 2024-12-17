using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Civics.Misc;
using Eco.Mods.LawfulMod.CivicsImpExp;
using Eco.Shared.Items;
using LawfulMod.Data;
using LawfulMod.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/law")]
    public class LawController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public LawDto[] GetLaws()
        {
            var laws = Registrars.Get<Law>().Where(l => l.State == ProposableState.Active || l.State == ProposableState.Draft);
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

        [HttpGet("{lawId}/sections")]
        public LawSectionDto[] GetSection(int lawId)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                return law.Sections.Select((s, index) => new LawSectionDto(law.Id, index, s.Title, TextUtils.StripTags(s.Description()), s.UserDescription, this.CanStore(lawId, index))).ToArray();
            }
            else
            {
                return new LawSectionDto[0];
            }
        }

        [HttpPost("{lawId}/sections/import/{sectionId}")]
        public IActionResult ImportSection(int lawId, int sectionId, [FromBody]ReferenceDto[]? references)
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
            var context = new ImportContext();
            references?.ToList().ForEach(r => context.ReferenceNameMap.Add((r.Type, r.Name), r.MappedName));
            var sectionObj = context.DeserialiseGenericObject(bundle.Civics.First().Data, typeof(LawSection));
            law.Sections.Add((LawSection)sectionObj);
            return Ok("");
        }
    }
}
