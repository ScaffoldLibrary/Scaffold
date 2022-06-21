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

namespace Scaffold.Launcher
{
    public class ScaffoldManager
    {
        public ScaffoldManager()
        {
            _scaffoldManifest = ScaffoldManifest.Fetch();
            _projectManifest = ProjectManifest.Fetch();
            _dependecyValidation = new DependencyValidator(_scaffoldManifest, _projectManifest);
            _moduleInstaller = new ModuleInstaller(_projectManifest, _dependecyValidation);
        }

        private ScaffoldManifest _scaffoldManifest;
        private ProjectManifest _projectManifest;
        private DependencyValidator _dependecyValidation;
        private ModuleInstaller _moduleInstaller;

        public bool Busy => _operations.Count > 0;
        private List<string> _operations = new List<string>();

        public List<ScaffoldModule> GetModules()
        {
            return _scaffoldManifest.Modules;
        }

        public ScaffoldModule GetLauncher()
        {
            return _scaffoldManifest.Launcher;
        }

        public bool IsModuleInstalled(ScaffoldModule package)
        {
            return _projectManifest.Contains(package.Key);
        }

        public void InstallModule(ScaffoldModule package)
        {
            _moduleInstaller.Install(package, true);
        }

        public void UninstallModule(ScaffoldModule module)
        {
            _moduleInstaller.TryUninstall(module, true);
        }

        public bool CheckForMissingDependencies()
        {
            return _dependecyValidation.ValidateDependencies();
        }

        public void InstallMissingDependencies()
        {
            if (_dependecyValidation.ValidateDependencies(out List<ScaffoldModule> missing))
            {
                _moduleInstaller.Install(missing);
            }
        }

        public async void CheckForModuleUpdate(ScaffoldModule module)
        {
            ScaffoldModule updatedModule = await ModuleFetcher.GetModule(module.Key);
            module.UpdateModuleInfo(updatedModule);
        }

        public async void UpdateInstalledModule(ScaffoldModule module)
        {
            if(module.LatestVersion == module.InstalledVersion)
            {
                Debug.Log($"Installed version of {module.Name} is already the Latest");
                return;
            }

            string moduleGitPath = module.Path;
            AddRequest request = Client.Add(moduleGitPath);

            _operations.Add("updating");
            while (!request.IsCompleted)
            {
                await Task.Delay(100);
            }
            _operations.Remove("updating");

            if (request.Status != StatusCode.Success)
            {
                return;
            }
        }

        public async void UpdateManifest()
        {
            _operations.Add("ManifestUpdate");
            ScaffoldManifest newManifest = await ModuleFetcher.GetManifest();
            _operations.Remove("ManifestUpdate");
            ScaffoldManifest manifest = ScaffoldManifest.Fetch();

            if (newManifest.Hash == manifest.Hash)
            {
                Debug.Log("Manifest file is up to date");
                return;
            }

            foreach (ScaffoldModule module in newManifest.Modules)
            {
                if (!manifest.ContainsModule(module.Key))
                {
                    manifest.AddModule(module);
                    continue;
                }

                ScaffoldModule currentModule = manifest.GetModule(module.Key);
                if (currentModule.LatestVersion == module.LatestVersion)
                {
                    continue;
                }

                currentModule.UpdateModuleInfo(module);
            }
        }
    }
}
