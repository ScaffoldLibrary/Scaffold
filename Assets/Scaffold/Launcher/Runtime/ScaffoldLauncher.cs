using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Launcher.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Scaffold.Launcher.PackageHandler;

namespace Scaffold.Launcher
{
    public class ScaffoldLauncher
    {
        public ScaffoldLauncher()
        {
            _modules = PackageUtilities.GetPackageModules();
        }

        private PackageModules _modules;

        public List<PackagePath> GetPackages()
        {
            return _modules.Modules;
        }

        public bool IsPackageInstalled(PackagePath package)
        {
            PackageManifest manifest = PackageUtilities.GetProjectManifest();
            return manifest.Contains(package.Key);
        }
        public void InstallPackage(PackagePath package)
        {
            //try Install
            //Show Popup
        }

        public void UpdateModules()
        {
            string moduleUrl = PackageUtilities.RawModuleGit;
            PackageModules modules = PackageUtilities.GetPackageModules();
            GitFetcher.Fetch<string>(moduleUrl, onRequestCompleted: Callback);

            void Callback(string rawData)
            {
                JObject rawModules = JObject.Parse(rawData);
                modules.Modules = rawModules["Packages"].ToObject<List<PackagePath>>();
                File.WriteAllText(PackageUtilities.RawModuleLocal, rawData);
            }
        }

        public bool CheckForMissingDependencies(out List<string> missing)
        {
            PackageManifest manifest = PackageUtilities.GetProjectManifest();
            List<PackagePath> installedModules =  manifest.FilterScaffoldModules();
            missing = installedModules.SelectMany(m => m.dependencies)
                                           .Distinct()
                                           .Where(d => !installedModules.Any(m => m.Key == d))
                                           .ToList();
            return missing != null && missing.Count > 0;
        }

        public void InstallPackages(List<string> packages)
        {
            PackageInstaller installer = new PackageInstaller(PackageUtilities.ManifestLocal, _modules);
            installer.Install(packages);
        }

        public void InstallPackage(string package)
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

    public enum LauncherStatus
    {

    }
}
