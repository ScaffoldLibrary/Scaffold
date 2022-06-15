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
    public partial class ScaffoldManager
    {
        public ScaffoldManager()
        {
            _modules = ScaffoldManifest.Fetch();
        }

        private ScaffoldManifest _modules;

        public List<ScaffoldModule> GetPackages()
        {
            return _modules.Modules;
        }

        public bool IsPackageInstalled(ScaffoldModule package)
        {
            ProjectManifest manifest = new ProjectManifest();
            return manifest.Contains(package.Key);
        }

        public void InstallPackage(ScaffoldModule package)
        {
            List<string> dependencies = new List<string>(package.dependencies) { package.Key };
            foreach(string dependency in dependencies){
                Debug.Log(dependency);
            }
            InstallPackages(dependencies);
            //try Install
            //Show Popup
        }

        public void UpdateModules()
        {
            string moduleUrl = PackageUtilities.RawModuleGit;
            ScaffoldManifest modules = ScaffoldManifest.Fetch();
            GitFetcher.Fetch<string>(moduleUrl, onRequestCompleted: Callback);

            void Callback(string rawData)
            {
                JObject rawModules = JObject.Parse(rawData);
                modules.Modules = rawModules["Modules"].ToObject<List<ScaffoldModule>>();
                File.WriteAllText(PackageUtilities.RawModuleLocal, rawData);
            }
        }

        public bool CheckForMissingDependencies(out List<string> missing)
        {
            ProjectManifest manifest = ProjectManifest.Fetch();
            List<ScaffoldModule> installedModules =  manifest.FilterScaffoldModules();
            missing = installedModules.SelectMany(m => m.dependencies)
                                           .Distinct()
                                           .Where(d => !installedModules.Any(m => m.Key == d))
                                           .ToList();
            return missing != null && missing.Count > 0;
        }

        private void InstallPackages(List<string> packages)
        {
            PackageInstaller installer = new PackageInstaller(PackageUtilities.ManifestLocal, _modules);
            installer.Install(packages);
        }

        private void InstallPackage(string package)
        {
            InstallPackages(new List<string>() { package });
        }

        public void InstallMissingDependencies()
        {
            if(CheckForMissingDependencies(out List<string> missing))
            {
                InstallPackages(missing);
            }
        }
    }
}
