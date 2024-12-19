// LawfulMod/API/UserController.cs
using Microsoft.AspNetCore.Mvc;
using LawfulMod.Data; // Ensure this namespace contains UserDto

namespace LawfulMod.API
{
    [Route("api/v1/lawful/user")]
    public class UserController : BaseController
    {
        [HttpGet]
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