using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scaffold.Core.Launcher.Utilities;

namespace Scaffold.Core.Launcher
{
    public static class PackageUtilities
    {

        private static PackageModules Modules;
        public static PackageModules GetPackageModules()
        {
            if (Modules == null)
            {
                Modules = Resources.Load<PackageModules>("Modules");
            }
            return Modules;
        }

        public static List<PackagePath> FilterScaffoldModules(this PackageManifest manifest)
        {
            PackageModules modules = GetPackageModules();
            var currentModules = manifest.dependencies
                                .Where(dependency => modules.ContainModule(dependency.Key))
                                .Select(depency => modules.GetPackage(depency.Key))
                                .ToList();
            return currentModules;
        }

        public static List<PackagePath> CreateDependencyGraph()
        {
            return null;
        }

        public static void GetModuleManifest(this PackagePath package, Action<PackageManifest> callback)
        {
            string path = package.manifestPath;
            GitFetcher.Fetch<PackageManifest>(path, onRequestCompleted: callback);
        }

        public static void GetModuleDependencies(this PackagePath package, Action<List<PackagePath>> callback)
        {
            string path = package.manifestPath;
            GitFetcher.Fetch<PackageManifest>(path, onRequestCompleted: Callback);

            void Callback(PackageManifest manifest)
            {
                callback?.Invoke(FilterScaffoldModules(manifest));
            }
        }

    }
}