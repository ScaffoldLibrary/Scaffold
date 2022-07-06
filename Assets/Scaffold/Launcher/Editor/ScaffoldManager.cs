using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Launcher.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Scaffold.Launcher.PackageHandler;
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
        public ScaffoldManager(ScaffoldLibrary library, ModuleInstaller installer, ModuleUpdater updater, DependencyValidator validator)
        {
            _library = library;
            _installer = installer;
            _updater = updater;
            _validator = validator;
        }

        private ScaffoldLibrary _library;
        private ModuleInstaller _installer;
        private ModuleUpdater _updater;
        private DependencyValidator _validator;

        private List<Module> _missingDependencies = new List<Module>();

        public List<Module> GetModules()
        {
            return _library.Modules;
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
            _installer.TryUninstall(module, true);
        }

        public void UpdateModule(Module module)
        {
            _updater.Update(module);
        }

        public async void CheckForModuleUpdates(Module module)
        {
            module = await ModuleFetcher.GetModule(module.name);
            _updater.UpdateInfo(module);
        }

        public async void UpdateLibrary()
        {
            ScaffoldLibrary library = await ModuleFetcher.GetManifest();
            //do stuff
        }
        
        public bool CheckForMissingDependencies()
        {
            return _validator.ValidateDependencies(out _missingDependencies);
        }
    }
}
