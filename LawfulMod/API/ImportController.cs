using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Mods.LawfulMod.CivicsImpExp;
using LawfulMod.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/import")]
    public class ImportController : Controller
    {
        [HttpPost]
        public string ImportSection(int lawId, int sectionId)
        {
            var section = LawfulPlugin.Obj.Db?.GetCollection<SectionDocument>("sections").FindById(sectionId);
            var bundle = CivicBundle.LoadFromText(section.JSON);
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            
            var sectionObj = new ImportContext().DeserialiseGenericObject(bundle.Civics.First().Data, typeof(LawSection));
            law.Sections.Add((LawSection)sectionObj);
            return "";
        }
    }
}
