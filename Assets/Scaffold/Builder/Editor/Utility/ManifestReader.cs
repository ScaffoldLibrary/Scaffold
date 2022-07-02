using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scaffold.Builder.Utilities
{
    public class ManifestReader
    {
        public static List<string> GetModuleDependencies()
        {
            IDictionary<string, JToken> dependencies = GetManifest();
            return dependencies.Where(kp => IsValidModuleKey(kp.Key))
                                           .Select(kp => kp.Key)
                                           .ToList();
        }

        private static IDictionary<string, JToken> GetManifest()
        {
            string manifestPath = "./Packages/manifest.json";
            string manifest = File.ReadAllText(manifestPath);
            JObject manifestToken = JObject.Parse(manifest);
            JObject dependencies = JObject.FromObject(manifestToken["dependencies"]);
            return dependencies;
        }

        private static bool IsValidModuleKey(string key)
        {
            return key.Contains("scaffold") && !key.Contains("scaffold.builder") && !key.Contains("scaffold.launcher");
        }
    }
}