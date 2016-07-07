using System.Configuration;
using System.Web;

namespace OctopusLibrary
{
    public class ConfigHandler
    {
        public static void Save(string Key, string Value)
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            config.AppSettings.Settings.Remove(Key);
            config.AppSettings.Settings.Add(Key, Value);
            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
