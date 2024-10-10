
using Eco.Gameplay.Players;
using Eco.WebServer.Web.Authentication;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected User? ContextUser => (HttpContext.User.Identity as EcoUserIdentity)?.User;
}