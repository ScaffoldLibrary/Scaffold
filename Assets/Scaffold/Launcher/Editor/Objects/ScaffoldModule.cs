using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Launcher.PackageHandler
{
    [System.Serializable]
    public class ScaffoldModule
    {
        public ScaffoldModule(/*string name, string description, string version, List<string> dependencies*/)
        {
            //Name = name;
            //UpdateModuleInfo(description, version, dependencies);
        }

        [Header("Details")]
        public string Name;
        public string Key;
        [TextArea]
        public string Description;

        [Header("Paths")]
        public string Path;
        public string Manifest;

        [Header("Versioning")]
        public string LatestVersion;
        public string InstalledVersion;

        [Header("Dependencies")]
        public List<string> Dependencies = new List<string>();

        public void UpdateModuleInfo(string description, string version, List<string> dependencies)
        {
            Description = description;
            LatestVersion = version;
            Dependencies = dependencies;
        }

        public static ScaffoldModule Create(string packageManifest)
        {
            return new ScaffoldModule();
        }
    }
}