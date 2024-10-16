﻿using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Civics.Misc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/law")]
    public class LawController : BaseController
    {
        public record LawDto(int Id, string Title, string Description, string Creator, String State, string Settlement, string? HostObject);

        [HttpGet]
        [AllowAnonymous]
        public LawDto[] GetLaws()
        {
            var laws = Registrars.Get<Law>().Where(l => l.Active());
            return laws.Select(l => GetDto(l)).ToArray();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetLaw(int id)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                return Ok(GetDto(law));
            }
            else
            {
                return NotFound();
            }
        }

        private LawDto GetDto(Law law)
        {
            return new LawDto(law.Id, law.Name, law.UserDescription, law.Creator.Name, law.State.ToString(), law.Settlement.Name, law.HostObject.Object?.Name);
        }
    }
}
