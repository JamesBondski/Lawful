using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Moose.Utils.TextUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/section")]
    public class SectionController : Controller
    {
        public record SectionDto(int LawId, int Index, string Title, string Description, string UserDescription);

        [HttpGet]
        [AllowAnonymous]
        public SectionDto[] GetSection(int lawId)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                int index = 0;
                return law.Sections.Select(s => new SectionDto(law.Id, index++, s.Title, TextUtils.StripTags(s.Description()), s.UserDescription)).ToArray();
            }
            else
            {
                return new SectionDto[0];
            }
        }
    }
}
