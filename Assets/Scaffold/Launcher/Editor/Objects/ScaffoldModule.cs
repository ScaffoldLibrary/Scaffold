using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Launcher.Objects
{
    [System.Serializable]
    public class ScaffoldModule
    {
        [Header("Details")]
        public string Key;
        public string Name;
        [TextArea]
        public string Description;

        [Header("Paths")]
        public string Path;

        [Header("Versioning")]
        public string LatestVersion;
        public string InstalledVersion;

        [Header("Dependencies")]
        public List<string> Dependencies = new List<string>();

        public void UpdateModuleInfo(ScaffoldModule updatedModule)
        {
            Name = updatedModule.Name;
            Description = updatedModule.Description;
            Path = updatedModule.Path;
            LatestVersion = updatedModule.LatestVersion;
            Dependencies = updatedModule.Dependencies;
        }
    }
}