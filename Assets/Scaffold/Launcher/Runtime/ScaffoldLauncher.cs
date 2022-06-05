﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Launcher.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Scaffold.Launcher
{
    public class ScaffoldLauncher: MonoBehaviour
    {
        public void Init()
        {

        }

        public void CheckForMissingDependencies()
        {
            PackageUtilities.GetPackageModules();
        }

        [ContextMenu("Update")]
        public void UpdateModules()
        {
            string moduleUrl = PackageUtilities.RawModuleGit;
            PackageModules modules = PackageUtilities.GetPackageModules();
            GitFetcher.Fetch<string>(moduleUrl, onRequestCompleted: Callback);

            void Callback(string rawData)
            {
                JObject rawModules = JObject.Parse(rawData);
                modules.Packages = rawModules["Packages"].ToObject<List<PackagePath>>();
                File.WriteAllText(PackageUtilities.RawModuleLocal, rawData);
            }
        }
    }
}
