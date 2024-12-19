using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/user")]
    public class UserController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<UserDto> GetUser()
        {
            if (ContextUser == null)
            {
                return Unauthorized();
            }

            var userDto = new UserDto(ContextUser.IsAdmin, ContextUser.AllCitizenships.Select(c => c.Name).ToArray());

            return Ok(userDto);
        }
    }
}