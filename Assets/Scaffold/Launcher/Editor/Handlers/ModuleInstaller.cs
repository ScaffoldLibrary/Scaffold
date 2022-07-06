using System.Collections.Generic;
using UnityEditor;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;

namespace Scaffold.Launcher.PackageHandler
{
    public class ModuleInstaller
    {
        public ModuleInstaller(Manifest manifest, DependencyValidator dependencies, FileService files)
        {
            _manifest = manifest;
            _dependencies = dependencies;
        }

        private Manifest _manifest;
        private DependencyValidator _dependencies;

        public void Install(List<Module> modules)
        {
            foreach (Module module in modules)
            {
                Install(module);
            }
            //_manifest.Save();
        }

        public void Install(Module module)
        {
            //if (!_manifest.Contains(module.name))
            //{
            //    _manifest.dependencies.Add(module.name, module.path);
            //    if (saveOnInstall)
            //    {
            //       // _manifest.Save();
            //    }
            //}
        }

        public void TryUninstall(Module module, bool saveOnUninstall)
        {
            //if (!_manifest.Contains(module.name))
            //{
            //    Debug.Log($"Tried to uninstall a module that was never installed {module.name}");
            //    return;
            //}

            if(_dependencies.CheckForDependingModules(module, out List<Module> dependers))
            {
                bool confirm = EditorUtility.DisplayDialog($"Uninstalling {module.name}", "There seems to be some dependencies on this Module, do you wish to uninstall anyway?", "Yes", "No");
                if (!confirm) return;
            }

            _manifest.dependencies.Remove(module.name);
            DefinesHandler.RemoveDefines(module.installDefines);
            if (saveOnUninstall)
            {
                //_manifest.Save();
            }
        }
    }
}

