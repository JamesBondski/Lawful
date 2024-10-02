using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Logging;
using Eco.Shared.Localization;

namespace LawfulMod
{
    public class LawfulPlugin : IInitializablePlugin, IModKitPlugin, IWebPlugin
    {
        public string GetCategory()
        {
            return "Civics";
        }

        public string? GetEmbeddedResourceNamespace()
        {
            return "LawfulMod.www";
        }

        public string? GetFontAwesomeIcon()
        {
            return null;
        }

        public LocString GetMenuTitle()
        {
            return new LocString("Law Manager");
        }

        public string? GetPluginIndexUrl()
        {
            return "LawfulPlugin/index.html";
        }

        public string? GetStaticFilesPath()
        {
            return null;
        }

        public string GetStatus()
        {
            return "Idle";
        }

        public void Initialize(TimedTask timer)
        {
            Log.WriteLine(new LocString("LawfulPlugin initialized"));
        }
    }
}
