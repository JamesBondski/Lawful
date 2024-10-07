using Eco.Gameplay.Players;

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
}
