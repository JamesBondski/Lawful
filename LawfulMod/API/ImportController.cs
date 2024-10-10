using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.LawfulMod.CivicsImpExp;
using LawfulMod.Data;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/import")]
    public class ImportController : BaseController
    {
        [HttpPost]
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

            if (!AuthorizationHelper.CanImport(ContextUser, law, section))
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
