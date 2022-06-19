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
        public const string RawModuleGit = "https://github.com/MgCohen/Scaffold-Launcher/raw/main/Assets/Scaffold/Launcher/Editor/Resources/RawModules.json";
        public const string RawModuleLocal = "./Assets/Scaffold/Launcher/Editor/Resources/RawModules.json";
        public const string TestFile = "./Assets/Scaffold/Launcher/Editor/Resources/TestFile.json";
        public const string ManifestLocal = "./Packages/manifest.json";

        public static List<ScaffoldModule> FilterScaffoldModules(this ProjectManifest manifest)
        {
            ScaffoldManifest modules = ScaffoldManifest.Fetch();
            var currentModules = manifest.dependencies
                                .Where(dependency => modules.ContainModule(dependency.Key))
                                .Select(depency => modules.GetModule(depency.Key))
                                .ToList();
            return currentModules;
        }

        public static void GetModuleManifest(this ScaffoldModule package, Action<ProjectManifest> callback)
        {
            string path = package.Path;
            GitFetcher.Fetch(path, onRequestCompleted: callback);
        }

        public static void GetModuleDependencies(this ScaffoldModule package, Action<List<ScaffoldModule>> callback)
        {
            string path = package.Path;
            GitFetcher.Fetch<ProjectManifest>(path, onRequestCompleted: Callback);

            void Callback(ProjectManifest manifest)
            {
                callback?.Invoke(FilterScaffoldModules(manifest));
            }
        }

    }
}