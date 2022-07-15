using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Launcher.Library;
using Scaffold.Launcher.Workers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Runners
{
    internal class LauncherValidation
    {
        private const string ManifestPath = "./Packages/manifest.json";
        private const string ValidationKey = "PROJECTVALIDATED";

        [InitializeOnLoadMethod]
        private static void ValidateProject()
        {
            bool isValidated = GetKey(ValidationKey, false);
            if (isValidated)
            {
                return;
            }

            if (CheckProjectDependencies())
            {
                SetKey(ValidationKey, true);
            }
        }

        private static bool CheckProjectDependencies()
        {
            FileService files = new FileService();
            Manifest projectManifest = files.Read<Manifest>(ManifestPath);
            ScaffoldLibrary scaffoldManifest = ScaffoldLibrary.Load();


            DependencyHandler validator = new DependencyHandler(scaffoldManifest, projectManifest);
            bool hasMissingDependencies = validator.ValidateDependencies();
            if (hasMissingDependencies)
            {
                Debug.Log("Project is missing a few Scaffold Modules, initializing Launcher to handle missing dependencies");
                ScaffoldLauncher.Launch();
                return false;
            }
            else
            {
                Debug.Log("All Package dependencies are installed!");
                return true;
            }
        }

        private static bool GetKey(string key, bool defaultValue)
        {
            int defaulted = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(key, defaulted) == 1;
        }

        private static void SetKey(string key, bool state)
        {
            int value = state ? 1 : 0;
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }
}
