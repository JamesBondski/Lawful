using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eco.Moose.Utils.TextUtils;

namespace LawfulMod.API
{
    [Route("api/v1/lawful/law")]
    public class LawController
    {
        public record LawDto(int Id, string Title, string Description, string Creator, String State, string Settlement, string HostObject);

        [HttpGet]
        [AllowAnonymous]
        public LawDto[] GetLaws()
        {
            var laws = Registrars.Get<Law>().All();
            return laws.Select(l => GetDto(l)).ToArray();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public LawDto GetLaw(int id)
        {
            var law = Registrars.Get<Law>().FirstOrDefault(l => l.Id == id);
            if (law != null)
            {
                return GetDto(law);
            }
            else
            {
                return null;
            }
        }

        private LawDto GetDto(Law law)
        {
            return new LawDto(law.Id, law.Name, law.UserDescription, law.Creator.Name, law.State.ToString(), law.Settlement.Name, law.HostObject.Object.Name);
        }
    }
}
