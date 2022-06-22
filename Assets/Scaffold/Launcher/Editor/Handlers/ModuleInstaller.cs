using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Scaffold.Launcher.Objects;
using UnityEditor.PackageManager;
using Scaffold.Launcher.Editor;

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
                _manifest.dependencies.Add(module.Key, module.Path);
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
                bool confirm = Popup.Assert($"Uninstalling {module.Key}", "There seems to be some dependencies on this Module, do you wish to uninstall anyway?", "Yes", "No");
                if (!confirm) return;
            }

            _manifest.dependencies.Remove(module.Key);
            DefinesHandler.RemoveDefines(module.Define);
            if (saveOnUninstall)
            {
                _manifest.Save();
            }
        }
    }
}

