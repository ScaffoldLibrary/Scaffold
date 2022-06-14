using Newtonsoft.Json;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.PackageHandler
{
    public class ScaffoldBuilderMenu
    {
        [MenuItem("Scaffold/Launcher/Update Scaffold Manifest")]
        public static void BuildScaffold()
        {
            ScaffoldBuilder builder = new ScaffoldBuilder();
            builder.UpdateModuleFile();
        }
    }

    public class ScaffoldBuilder
    {
        private int requestCount = 0;
        private bool _buildingGraph = false;
        private string _rawFilePath = "./Assets/Scaffold/Launcher/Runtime/Resources/RawModules.json";
        private Dictionary<string, List<string>> _dependencyGraph = new Dictionary<string, List<string>>();

        public void UpdateModuleFile()
        {
            if (_buildingGraph == true)
            {
                Debug.LogWarning("Tried to build new graph while still building, please wait");
                return;
            }

            _buildingGraph = true;
            _dependencyGraph = new Dictionary<string, List<string>>();
            ScaffoldManifest modules = PackageUtilities.GetPackageModules();
            List<PackagePath> packages = modules.Modules;

            requestCount = packages.Count;
            foreach (PackagePath package in packages)
            {
                package.GetModuleDependencies((list) => AddToGraph(package, list));
            }
            TryResolveCircularDependencies();
        }
        private void AddToGraph(PackagePath rootPackage, List<PackagePath> packages)
        {
            if (packages == null)
            {
                Debug.LogWarning("Failed to load dependencies of package");
            }

            requestCount--;
            string packageKey = rootPackage.Key;
            if (!_dependencyGraph.ContainsKey(packageKey))
            {
                _dependencyGraph.Add(packageKey, new List<string>());
            }

            List<string> dependencyKeys = packages.Select(dependency => dependency.Key).ToList();
            _dependencyGraph[packageKey].AddRange(dependencyKeys);
            TryResolveCircularDependencies();
        }
        private void TryResolveCircularDependencies()
        {
            if (requestCount > 0)
            {
                return;
            }
            ResolveCircularDependencies();
        }
        private void ResolveCircularDependencies()
        {
            for (int i = _dependencyGraph.Count - 1; i >= 0; i--)
            {
                var entry = _dependencyGraph.ElementAt(i);
                List<string> newDependencies = BuildPackageTree(entry.Value);
                _dependencyGraph[entry.Key] = newDependencies;
            }

            BuildModuleFile();
        }
        private void BuildModuleFile()
        {
            Debug.Log("Building File");
            ScaffoldManifest modules = PackageUtilities.GetPackageModules();
            List<PackagePath> packages = modules.Modules;
            foreach (PackagePath package in packages)
            {
                package.dependencies = _dependencyGraph[package.Key];
            }

            string json = JsonConvert.SerializeObject(modules, Formatting.Indented);
            File.WriteAllText(_rawFilePath, json);
            _buildingGraph = false;
        }
        private List<string> BuildPackageTree(List<string> entry)
        {
            List<string> closedList = new List<string>();
            List<string> openList = new List<string>(entry);

            while (openList.Count > 0)
            {
                List<string> aggregateList = new List<string>();
                foreach (string key in openList)
                {
                    aggregateList.AddRange(_dependencyGraph[key]);
                }

                closedList.AddRange(openList);
                openList = aggregateList.Distinct().Where(d => !closedList.Contains(d) || !openList.Contains(d)).ToList();
            }
            return closedList;
        }
    }
}