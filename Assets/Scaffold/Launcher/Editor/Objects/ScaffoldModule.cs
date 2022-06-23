using Newtonsoft.Json.Linq;
using Scaffold.Launcher.PackageHandler;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Objects
{
    [System.Serializable]
    public class ScaffoldModule
    {
        [Header("Details")]
        public string Key;
        public string Name;
        [TextArea]
        public string Description;

        [Header("Paths")]
        public string Path;
        public string Define;

        [Header("Versioning")]
        public string LatestVersion;
        public string InstalledVersion
        {
            get
            {
                string path = $"Packages/{Key}/package.json";
                if (!File.Exists(path))
                {
                    return "0.0.0";
                }
                return GetInstalledVersion();
            }
        }

        [Header("Dependencies")]
        public List<string> Dependencies = new List<string>();

        public void UpdateModuleInfo(ScaffoldModule updatedModule)
        {
            Name = updatedModule.Name;
            Description = updatedModule.Description;
            Path = updatedModule.Path;
            LatestVersion = updatedModule.LatestVersion;
            Dependencies = updatedModule.Dependencies;
        }

        private string GetInstalledVersion()
        {
            string path = $"Packages/{Key}/package.json";
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                JObject token = JObject.Parse(content);
                string version = token["version"].ToString();
                return version;
            }
            else
            {
                return "Installing...";
            }
        }

        private bool IsOutdated(string version)
        {
            Version installed = new Version(version);
            Version latest = new Version(LatestVersion);
            return installed < latest;
        }

        public bool IsInstalled()
        {
            if (!DefinesHandler.CheckDefines(Define))
            {
                return false;
            }

            string path = $"Packages/{Key}/package.json";
            if (!File.Exists(path))
            {
                return false;
            }

            return true;
        }

        public bool IsOutdated()
        {
            if (!IsInstalled())
            {
                return false;
            }

            Version installed = new Version(InstalledVersion);
            Version latest = new Version(LatestVersion);
            return installed < latest;
        }
    }
}