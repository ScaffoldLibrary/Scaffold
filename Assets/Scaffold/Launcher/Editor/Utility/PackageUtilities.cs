using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Scaffold.Launcher.PackageHandler;
using System.Net.Http;

namespace Scaffold.Launcher.Utilities
{
    public static class PackageUtilities
    {
        public const string LauncherGit = "https://github.com/MgCohen/Scaffold-Launcher.git?path=/Assets/Scaffold/Launcher";
        public const string RawModuleGit = "https://github.com/MgCohen/Scaffold-Launcher/raw/main/Assets/Scaffold/Launcher/Editor/Resources/RawModules.json";
        public const string RawModuleLocal = "./Assets/Scaffold/Launcher/Editor/Resources/RawModules.json";
        public const string TestFile = "./Assets/Scaffold/Launcher/Editor/Resources/TestFile.json";

        public const string ManifestUrl = "https://e227iwvnp2dov6ddpl3ujveyyi0vahcu.lambda-url.us-east-1.on.aws/";

        public static List<ScaffoldModule> FilterScaffoldModules(this ProjectManifest manifest)
        {
            ScaffoldManifest modules = ScaffoldManifest.Fetch();
            var currentModules = manifest.Dependencies
                                .Where(dependency => modules.ContainsModule(dependency.Key))
                                .Select(depency => modules.GetModule(depency.Key))
                                .ToList();
            return currentModules;
        }

        public async static void FetchNewScaffoldManifest()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(ManifestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            string content = response.Content.ReadAsStringAsync().Result;
            ScaffoldManifest manifest = ScaffoldManifest.Fetch();
            ScaffoldManifest newManifest = JsonConvert.DeserializeObject<ScaffoldManifest>(content);

            if(newManifest.Hash == manifest.Hash)
            {
                Debug.Log("Manifest file is up to date");
                return;
            }

            foreach(ScaffoldModule module in newManifest.Modules)
            {
                if (!manifest.ContainsModule(module.Key))
                {
                    manifest.AddModule(module);
                    continue;
                }

                ScaffoldModule currentModule = manifest.GetModule(module.Key);
                if(currentModule.LatestVersion == module.LatestVersion)
                {
                    continue;
                }

                currentModule.UpdateModuleInfo(module);
            }
        }

    }
}