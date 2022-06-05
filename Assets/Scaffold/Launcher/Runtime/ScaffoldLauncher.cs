using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Core.Launcher.ModuleHandler;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Scaffold.Core.Launcher
{


    public class ScaffoldLauncher : MonoBehaviour
    {
        public List<PackagePath> paths = new List<PackagePath>();

        private PackageModules _modules;


        //TODO: Update file based on git
        public void InitLauncher()
        {
            _modules = Resources.Load<PackageModules>("Modules");
            if (_modules == null) return;
            PackageManifest manifest = GetManifest();
            List<PackagePath> currentModules = manifest.FilterScaffoldModules();

        }

        //TODO: Fetch current *PROJECT* manifest
        private PackageManifest GetManifest()
        {
            string path = ".Packages/manifest.json";
            string manifestText = File.ReadAllText(path);
            PackageManifest manifest = JsonConvert.DeserializeObject<PackageManifest>(manifestText);
            return manifest;
        }

        //TODO: Fetch manifest of each installed module
        private void GetScaffoldModuleManifest(PackagePath package)
        {
            string path = package.manifestPath;
            //_gitFetcher.Fetch<PackageManifest>(path, onRequestCompleted: GetModuleDependencies);
        }

        private void GetModuleDependencies(PackageManifest manifest)
        {

        }
        //TODO: Filter pending modules
        //TODO: Create Graph
        //TODO: Add needed modules to project
        //TODO: Create UI


    }
}

//[ContextMenu("Read")]
//public void Read()
//{
//    string path = "./Assets/Scaffold/Launcher/Modules.json";
//    var reader = new ModuleReader(path);
//    var file = reader.ReadFile();
//    foreach (var entry in file)
//    {
//        Debug.Log($"{entry.Key} : {entry.Value}");
//    }
//}


//[ContextMenu("Fetch")]
//public void FetchModules()
//{
//    string path = "./Assets/Scaffold/Launcher/Modules.json";
//    var reader = new ModuleReader(path);
//    var file = reader.ReadFile();

//    foreach (var entry in file)
//    {
//        paths.Add(new PackagePath(entry.Key, entry.Value));
//    }
//}

//[ContextMenu("Write")]
//public void Write()
//{
//    string path = "./Assets/Scaffold/Launcher/Modules.json";
//    var reader = new ModuleReader(path);
//    var writer = new ModuleWriter(reader, path);
//    writer.Write(paths.ToArray());
//}
