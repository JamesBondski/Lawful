using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using LawfulMod.Util;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/section")]
    public class SectionController : BaseController
    {
        public record SectionDto(int LawId, int Index, string Title, string Description, string UserDescription, bool CanStore);

        [HttpGet]
        public SectionDto[] GetSection(int lawId)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId);
            if (law != null)
            {
                return law.Sections.Select((s, index) => new SectionDto(law.Id, index, s.Title, TextUtils.StripTags(s.Description()), s.UserDescription, AuthorizationHelper.CanStore(ContextUser, lawId, index))).ToArray();
            }
            else
            {
                return new SectionDto[0];
            }
        }
    }
}
