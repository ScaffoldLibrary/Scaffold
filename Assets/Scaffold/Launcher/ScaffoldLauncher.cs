using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Core.Launcher.ModuleHandler;

namespace Scaffold.Core.Launcher
{
    public class ScaffoldLauncher : MonoBehaviour
    {
        public List<PackagePath> paths = new List<PackagePath>();

        private void Start()
        {
            var m = Resources.Load<PackageModules>("Modules");
            Debug.Log(m.packages[0].path);
        }

        [ContextMenu("Read")]
        public void Read()
        {
            string path = "./Assets/Scaffold/Launcher/Modules.json";
            var reader = new ModuleReader(path);
            var file = reader.ReadFile();
            foreach (var entry in file)
            {
                Debug.Log($"{entry.Key} : {entry.Value}");
            }
        }


        [ContextMenu("Fetch")]
        public void FetchModules()
        {
            string path = "./Assets/Scaffold/Launcher/Modules.json";
            var reader = new ModuleReader(path);
            var file = reader.ReadFile();

            foreach(var entry in file)
            {
                paths.Add(new PackagePath(entry.Key, entry.Value));
            }
        }

        [ContextMenu("Write")]
        public void Write()
        {
            string path = "./Assets/Scaffold/Launcher/Modules.json";
            var reader = new ModuleReader(path);
            var writer = new ModuleWriter(reader, path);
            writer.Write(paths.ToArray());
        }

    }
}