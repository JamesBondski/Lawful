using Eco.Core.Systems;
using Eco.Gameplay.Civics.Laws;
using Eco.Gameplay.Civics.Misc;
using Eco.Gameplay.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulMod.API
{
    [Route("api/v1/lawful")]
    public class CivicsController : Controller
    {
        [HttpGet("laws")]
        [AllowAnonymous]
        public int[] GetStrings()
        {
            CivicsUtils.
            return Registrars.Get<Law>().All().Select(l => l.Id).ToArray();
        }

        [HttpGet("section")]
        [AllowAnonymous]
        public string[] GetSections(int lawId)
        {
            return Registrars.Get<Law>().FirstOrDefault(l => l.Id == lawId)?.Sections.Select(s => s.Title).ToArray() ?? Array.Empty<string>();
        }
    }
}
