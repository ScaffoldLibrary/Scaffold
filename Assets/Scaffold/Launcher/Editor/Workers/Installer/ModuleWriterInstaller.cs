using System.Collections.Generic;
using UnityEditor;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;
using System.Linq;
using Scaffold.Launcher.Utilities;
using UnityEditor.PackageManager;

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
            Client.Resolve();
        }

        public void Uninstall(Module module)
        {
            if (_dependencies.CheckForDependingModules(module, out List<Module> dependers))
            {
                bool confirm = EditorUtility.DisplayDialog($"Uninstalling {module.name}", "There seems to be some dependencies on this Module, do you wish to uninstall anyway?", "Yes", "No");
                if (!confirm) return;
            }

            RemoveDefines(module.installDefines);
            RemoveFromManifest(new List<Module>() { module });
        }

        public void Uninstall(List<Module> modules)
        {
            List<string> defines = new List<string>();
            foreach(Module module in modules)
            {
                defines.AddRange(module.installDefines);
            }

            RemoveDefines(defines);
            RemoveFromManifest(modules);
        }

        private void RemoveFromManifest(List<Module> modules)
        {
            foreach (Module module in modules)
            {
                _manifest.Remove(module.name);
            }
            _files.Save(_manifest);
            Client.Resolve();
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

        private void RemoveDefines(List<string> defines)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> projectDefines = definesString.Split(';').ToList();
            projectDefines = projectDefines.Except(defines).ToList();
            string defineString = string.Join(";", projectDefines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineString);
        }
    }
}

