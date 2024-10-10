using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
using LawfulMod.Data;

public static class AuthorizationHelper
{
    public static bool CanStore(User? user, int lawId, int sectionIndex)
    {
        if(user != null && user.IsAdmin)
        {
            return true;
        }
        return false;
    }

    public static bool CanImport(User? user, Law? law, SectionDocument? storedSection)
    {
        if(user != null && law != null && storedSection != null)
        {
            if (law.State == ProposableState.Draft && user.AllCitizenships.Any(c => c == law.Settlement))
            {
                return true;
            }
        }

        return false;
    }
}
