using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Scaffold.Launcher.PackageHandler
{
    public class PackageInstaller
    {
        public PackageInstaller(string path, PackageModules modules)
        {
            _manifestFilePath = path;
            _modules = modules;
        }

        private string _manifestFilePath;
        private PackageModules _modules;

        private IDictionary<string, string> GetManifest()
        {
            string text = File.ReadAllText(_manifestFilePath);
            JObject jObj = JObject.Parse(text);
            return jObj.ToObject<IDictionary<string, string>>();
        }

        public void Install(List<string> dependencies)
        {
            var manifest = GetManifest();
            foreach(var dependency in dependencies)
            {
                PackagePath package = _modules.GetPackage(dependency);
                manifest.Add(package.Key, package.Path);
            }

            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(_manifestFilePath, json);
        }
    }
}

