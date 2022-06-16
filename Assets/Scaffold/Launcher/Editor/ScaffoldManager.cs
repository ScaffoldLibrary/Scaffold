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

namespace Scaffold.Launcher
{
    public class ScaffoldManager
    {
        public ScaffoldManager()
        {
            _scaffoldManifest = ScaffoldManifest.Fetch();
            _projectManifest = ProjectManifest.Fetch();
        }

        private ScaffoldManifest _scaffoldManifest;
        private ProjectManifest _projectManifest;

        public List<ScaffoldModule> GetModules()
        {
            return _scaffoldManifest.Modules;
        }

        public bool IsPackageInstalled(ScaffoldModule package)
        {
            return _projectManifest.Contains(package.Key);
        }

        public void InstallPackage(ScaffoldModule package)
        {
            //List<string> dependencies = new List<string>(package.dependencies) { package.Key };
            //foreach(string dependency in dependencies){
            //    Debug.Log(dependency);
            //}

            ////TODO: Add install request popup here
            //InstallPackages(dependencies);
            //PackageInstaller installer = new PackageInstaller(_scaffoldManifest);
            //installer.Install(packages);
        }

        public void UpdateModules()
        {
            string moduleUrl = PackageUtilities.RawModuleGit;
            GitFetcher.Fetch<string>(moduleUrl, onRequestCompleted: Callback);

            void Callback(string rawData)
            {
                JObject rawModules = JObject.Parse(rawData);
                _scaffoldManifest.Modules = rawModules["Modules"].ToObject<List<ScaffoldModule>>();
                File.WriteAllText(PackageUtilities.RawModuleLocal, rawData);
            }
        }

        public bool CheckForMissingDependencies()
        {
            DependencyValidator validator = new DependencyValidator(_scaffoldManifest, _projectManifest);
            return validator.ValidateDependencies();
        }

        public void InstallMissingDependencies()
        {
            DependencyValidator validator = new DependencyValidator(_scaffoldManifest, _projectManifest);
            if(validator.ValidateDependencies(out List<ScaffoldModule> missing))
            {
                //install
            }
        }

    }
}
