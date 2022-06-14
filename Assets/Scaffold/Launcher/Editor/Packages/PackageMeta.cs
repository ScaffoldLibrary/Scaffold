using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Launcher.PackageHandler
{
    public class PackageManifest
    {
        public Dictionary<string, string> dependencies;

        public bool Contains(string package)
        {
            return dependencies.ContainsKey(package);
        }
    }

    [System.Serializable]
    public class PackagePath
    {
        public string Name;
        public string Key;
        public string Path;
        public string Version;
        public string Manifest;

        [TextArea] public string Description;

        public List<string> dependencies = new List<string>();
    }
}