using Newtonsoft.Json;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;

namespace Scaffold.Launcher.PackageHandler
{
    public class ProjectManifest
    {
        public static ProjectManifest Fetch()
        {
            string text = File.ReadAllText(PackageUtilities.ManifestLocal);
            return JsonConvert.DeserializeObject<ProjectManifest>(text);
        }

        public Dictionary<string, string> dependencies;

        public bool Contains(string package)
        {
            return dependencies.ContainsKey(package);
        }
    }
}