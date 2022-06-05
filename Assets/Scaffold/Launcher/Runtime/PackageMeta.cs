using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Core.Launcher
{
    public class PackageManifest
    {
        public Dictionary<string, string> dependencies;
    }

    [System.Serializable]
    public class PackagePath
    {
        public string Name;
        public string Key;
        public string Path;
        public string Manifest;

        public List<string> dependencies = new List<string>();
    }
}