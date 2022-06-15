using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;

namespace Scaffold.Launcher.PackageHandler
{
    public class PackageInstaller
    {
        public PackageInstaller(string path, ScaffoldManifest modules)
        {
            _manifestFilePath = path;
            _modules = modules;
        }

        private string _manifestFilePath;
        private ScaffoldManifest _modules;

        private JObject GetManifest()
        {
            string text = File.ReadAllText(_manifestFilePath);
            return JObject.Parse(text);
        }

        private IDictionary<string, string> GetDependency(JObject jObject)
        {
            Dictionary<string, string> rawManifest = jObject["dependencies"].ToObject<Dictionary<string, string>>();
            return rawManifest;
        }

        public void Install(List<string> dependencies)
        {
            JObject manifest = GetManifest();
            IDictionary<string, string> manifestDependencies = GetDependency(manifest);
            foreach (var dependency in dependencies)
            {
                ScaffoldModule package = _modules.GetPackage(dependency);
                if (manifestDependencies.ContainsKey(package.Key))
                {
                    continue;
                }
                manifestDependencies.Add(package.Key, package.Path);
            }
            manifest["dependencies"] = JToken.FromObject(manifestDependencies);
            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(_manifestFilePath, json);
        }
    }
}

