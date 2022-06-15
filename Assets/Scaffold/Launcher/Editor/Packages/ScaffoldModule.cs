using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Launcher.PackageHandler
{
    [System.Serializable]
    public class ScaffoldModule
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