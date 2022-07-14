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
using Scaffold.Launcher.Library;
using Scaffold.Launcher.Editor;
using UnityEditor;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;

namespace Scaffold.Launcher
{
    public class ScaffoldManager
    {
        public ScaffoldManager(Manifest manifest, ScaffoldLibrary library, IModuleInstaller installer, IModuleUpdater updater, DependencyHandler dependencies)
        {
            _manifest = manifest;
            _library = library;
            _dependencies = dependencies;
            _installer = installer;
            _updater = updater;
        }

        private Manifest _manifest;
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
            Dictionary<Module, Version> installedModules = new Dictionary<Module, Version>();
            List<Module> modules = _manifest.GetScaffoldDependencies().Select(d => _library.GetModule(d)).ToList();
            foreach(Module module in modules)
            {
                string moduleManifestPath = $"Packages/{module.name}/package.json";
                string content = AssetDatabase.LoadAssetAtPath<TextAsset>(moduleManifestPath).text;
                JObject serializedObj = JObject.Parse(content);
                Version version = new Version(serializedObj["version"].ToString());
                installedModules.Add(module, version);
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
            EditorUtility.SetDirty(_library);
            AssetDatabase.SaveAssets();
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
