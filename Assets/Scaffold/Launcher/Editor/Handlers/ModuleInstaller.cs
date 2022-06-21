using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Scaffold.Launcher.Objects;
using UnityEditor.PackageManager;

namespace Scaffold.Launcher.PackageHandler
{
    public class ModuleInstaller
    {
        public ModuleInstaller(ProjectManifest manifest, DependencyValidator dependencies)
        {
            _manifest = manifest;
            _dependencies = dependencies;
        }

        private ProjectManifest _manifest;
        private DependencyValidator _dependencies;

        public void Install(List<ScaffoldModule> modules)
        {
            foreach (ScaffoldModule module in modules)
            {
                Install(module, false);
            }
            _manifest.Save();
        }

        public void Install(ScaffoldModule module, bool saveOnInstall)
        {
            if (!_manifest.Contains(module.Key))
            {
                module.InstalledVersion = module.LatestVersion;
                _manifest.Dependencies.Add(module.Key, module.Path);
                if (saveOnInstall)
                {
                    _manifest.Save();
                }
            }
        }

        public void TryUninstall(ScaffoldModule module, bool saveOnUninstall)
        {
            if (!_manifest.Contains(module.Key))
            {
                Debug.Log($"Tried to uninstall a module that was never installed {module.Key}");
                return;
            }

            if(_dependencies.CheckForDependingModules(module, out List<ScaffoldModule> dependers))
            {
                //you sure you want to remove? other modules depende on this
            }

            _manifest.Dependencies.Remove(module.Key);
            if (saveOnUninstall)
            {
                _manifest.Save();
            }
        }
    }
}

