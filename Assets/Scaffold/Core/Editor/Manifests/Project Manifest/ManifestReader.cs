using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Scaffold.Core.Editor.Manifest
{
    /// <summary>
    /// Reads and Create a <c>Manifest</c> object from disk
    /// </summary>
    public class ManifestReader
    {
        public ManifestReader(string path)
        {
            _manifestPath = path;
        }

        private string _manifestPath;

        public Manifest GetManifest()
        {
            return null;
        }

        public Manifest GetFilteredManifest()
        {
            return null;
        }

        public List<string> GetModuleDependencies()
        {
            IDictionary<string, JToken> dependencies = GetRawManifest();
            return dependencies.Where(kp => IsValidModuleKey(kp.Key))
                                           .Select(kp => kp.Key)
                                           .ToList();
        }

        private IDictionary<string, JToken> GetRawManifest()
        {
            string manifestPath = "./Packages/manifest.json";
            string manifest = File.ReadAllText(manifestPath);
            JObject manifestToken = JObject.Parse(manifest);
            JObject dependencies = JObject.FromObject(manifestToken["dependencies"]);
            return dependencies;
        }

        private bool IsValidModuleKey(string key)
        {
            return key.Contains("scaffold") && !key.Contains("scaffold.builder") && !key.Contains("scaffold.launcher");
        }
    }
}