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
            if (_cachedManifest == null)
            {
                string text = File.ReadAllText(PackageUtilities.ManifestLocal);
                _cachedManifest = JsonConvert.DeserializeObject<ProjectManifest>(text);
            }

            return _cachedManifest;
        }
        private static ProjectManifest _cachedManifest;

        public Dictionary<string, string> dependencies;

        public bool Contains(string package)
        {
            return dependencies.ContainsKey(package);
        }

        public void Save()
        {
            string manifestText = JsonConvert.SerializeObject(this);
            File.WriteAllText(PackageUtilities.ManifestLocal, manifestText);
        }
    }
}