using System.Collections.Generic;
using UnityEditor;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;
using System.Linq;

namespace Scaffold.Launcher.Workers
{
    public class ModuleWriterInstaller : IModuleInstaller
    {
        public ModuleWriterInstaller(Manifest manifest, DependencyHandler dependencies, FileService files)
        {
            _manifest = manifest;
            _dependencies = dependencies;
            _files = files;
        }

        private Manifest _manifest;
        private DependencyHandler _dependencies;
        private FileService _files;

        public void Install(List<Module> modules)
        {
            List<Module> dependencies = new List<Module>();
            foreach (Module module in modules)
            {
                List<Module> moduleDependencies = _dependencies.GetModuleDependencies(module, true);
                dependencies.AddRange(moduleDependencies);
            }

            dependencies = dependencies.Distinct().ToList();
            AddToManifest(dependencies);
        }

        public void Install(Module module)
        {
            List<Module> dependencies = _dependencies.GetModuleDependencies(module, false);
            bool missingModules = HasMissingDependencies(dependencies);
            if (missingModules)
            {
                bool addDependencies = EditorUtility.DisplayDialog("Dependencies required", "this module has required dependencies, do you wish to install them?", "Yes", "No");
                if (addDependencies)
                {
                    dependencies.Add(module);
                    AddToManifest(dependencies);
                    return;
                }
            }
            AddToManifest(new List<Module>() { module });
        }

        private void AddToManifest(List<Module> modules)
        {
            foreach (Module module in modules)
            {
                _manifest.Add(module.name, module.path);
            }
            _files.Save(_manifest);
        }

        public void Uninstall(Module module)
        {
            if (_dependencies.CheckForDependingModules(module, out List<Module> dependers))
            {
                bool confirm = EditorUtility.DisplayDialog($"Uninstalling {module.name}", "There seems to be some dependencies on this Module, do you wish to uninstall anyway?", "Yes", "No");
                if (!confirm) return;
            }

            RemoveFromManifest(new List<Module>() { module });
            DefinesHandler.RemoveDefines(module.installDefines);
        }

        public void Uninstall(List<Module> modules)
        {
            List<string> defines = new List<string>();
            foreach(Module module in modules)
            {
                defines.AddRange(module.installDefines);
            }

            DefinesHandler.RemoveDefines(defines);
            RemoveFromManifest(modules);
        }

        private void RemoveFromManifest(List<Module> modules)
        {
            foreach (Module module in modules)
            {
                _manifest.Remove(module.name);
            }
            _files.Save(_manifest);
        }

        private bool HasMissingDependencies(List<Module> modules)
        {
            foreach (Module module in modules)
            {
                if (!_manifest.Contains(module.name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

