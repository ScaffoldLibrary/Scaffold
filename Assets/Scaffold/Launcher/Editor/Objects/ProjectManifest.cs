using Newtonsoft.Json;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Scaffold.Launcher.Objects
{
    public class ProjectManifest
    {
        public static ProjectManifest Fetch()
        {
            if (CachedManifest == null)
            {
                string text = File.ReadAllText(ManifestPath);
                CachedManifest = JsonConvert.DeserializeObject<ProjectManifest>(text);
            }

            return CachedManifest;
        }

        private static ProjectManifest CachedManifest;
        private const string ManifestPath = "./Packages/manifest.json";

        public Dictionary<string, string> Dependencies;

        public bool Contains(string package)
        {
            return Dependencies.ContainsKey(package);
        }

        public void Save()
        {
            string manifestText = JsonConvert.SerializeObject(this);
            File.WriteAllText(ManifestPath, manifestText);
            AssetDatabase.Refresh();
        }
    }
}