using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Launcher.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Scaffold.Launcher.Workers;
using System;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Threading.Tasks;
using Scaffold.Launcher.Objects;
using Scaffold.Launcher.Editor;
using UnityEditor;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;

namespace Scaffold.Launcher
{
    public class ScaffoldManager
    {
        public ScaffoldManager(ScaffoldLibrary library, IModuleInstaller installer, IModuleUpdater updater, DependencyHandler dependencies)
        {
            _library = library;
            _dependencies = dependencies;
            _installer = installer;
            _updater = updater;
        }

        private ScaffoldLibrary _library;
        private DependencyHandler _dependencies;
        private IModuleInstaller _installer;
        private IModuleUpdater _updater;

        private List<Module> _missingDependencies = new List<Module>();

        public List<Module> GetModules()
        {
            return _library.Modules;
        }

        public Dictionary<Module, Version> GetInstalledModules()
        {
            List<Module> modules = GetModules();
            List<Install> installs = _library.InstalledModules;
            Dictionary<Module, Version> installedModules = new Dictionary<Module, Version>();
            foreach (var install in installs)
            {
                Module module = modules.FirstOrDefault(m => m.name == install.ModuleName);
                if (module != null)
                {
                    installedModules.Add(module, install.Version);
                }
            }
            return installedModules;
        }

        public Module GetLauncher()
        {
            return _library.Launcher;
        }

        public void AddModule(Module module)
        {
            _installer.Install(module);
        }

        public void RemoveModule(Module module)
        {
            _installer.Uninstall(module);
        }

        public void UpdateModule(Module module)
        {
            _updater.UpdateModule(module);
        }

        public async void CheckForModuleUpdates(Module module)
        {
            module = await ModuleFetcher.GetModule(module.name);
            _updater.UpdateModuleInfo(module);
        }

        public async void UpdateLibrary()
        {
            ScaffoldLibrary library = await ModuleFetcher.GetManifest();
            _library.Modules = library.Modules;
            _library.Hash = library.Hash;
        }

        public bool CheckForMissingDependencies()
        {
            return _dependencies.ValidateDependencies(out _missingDependencies);
        }

        public void ResolveDependencies()
        {
            _installer.Install(_missingDependencies);   
        }
    }
}
