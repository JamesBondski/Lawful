using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
using Eco.WebServer.Web.Authentication;
using LawfulMod.Data;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    public class BaseController : Controller
    {
        protected User? ContextUser => (HttpContext.User.Identity as EcoUserIdentity)?.User;

        protected bool CanStore(int lawId, int sectionIndex)
        {
            if (this.ContextUser != null && this.ContextUser.IsAdmin)
            {
                return true;
            }
            return false;
        }

        protected bool CanImport(Law? law, SectionDocument? storedSection)
        {
            if (this.ContextUser != null && law != null && storedSection != null)
            {
                if ((law.State == ProposableState.Draft && this.ContextUser.AllCitizenships.Any(c => c == law.Settlement)) || this.ContextUser.IsAdmin)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool CanDelete(SectionDocument? storedSection)
        {
            if (this.ContextUser != null && storedSection != null)
            {
                return this.ContextUser.IsAdmin;
            }
            return false;
        }
    }
}