using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Scaffold.Launcher.PackageHandler;

namespace Scaffold.Launcher.Utilities
{
    public static class PackageUtilities
    {
        public const string LauncherGit = "https://github.com/MgCohen/Scaffold-Launcher.git?path=/Assets/Scaffold/Launcher";
        public const string RawModuleGit = "https://github.com/MgCohen/Scaffold-Launcher/raw/main/Assets/Scaffold/Launcher/Runtime/Resources/RawModules.json";
        public const string RawModuleLocal = "./Assets/Scaffold/Launcher/Runtime/Resources/RawModules.json";
        public const string TestFile = "./Assets/Scaffold/Launcher/Runtime/Resources/TestFile.json";
        public const string ManifestLocal = "./Packages/manifest.json";

        private static ScaffoldManifest Modules;

        public static ScaffoldManifest GetPackageModules()
        {
            if (Modules == null)
            {
                Modules = Resources.Load<ScaffoldManifest>("Modules");
            }
            return Modules;
        }

        public static PackageManifest GetProjectManifest()
        {
            string text = File.ReadAllText(ManifestLocal);
            return JsonConvert.DeserializeObject<PackageManifest>(text);
        }

        public static List<PackagePath> FilterScaffoldModules(this PackageManifest manifest)
        {
            ScaffoldManifest modules = GetPackageModules();
            var currentModules = manifest.dependencies
                                .Where(dependency => modules.ContainModule(dependency.Key))
                                .Select(depency => modules.GetPackage(depency.Key))
                                .ToList();
            return currentModules;
        }

        public static void GetModuleManifest(this PackagePath package, Action<PackageManifest> callback)
        {
            string path = package.Manifest;
            GitFetcher.Fetch(path, onRequestCompleted: callback);
        }

        public static void GetModuleDependencies(this PackagePath package, Action<List<PackagePath>> callback)
        {
            string path = package.Manifest;
            GitFetcher.Fetch<PackageManifest>(path, onRequestCompleted: Callback);

            void Callback(PackageManifest manifest)
            {
                callback?.Invoke(FilterScaffoldModules(manifest));
            }
        }

    }
}