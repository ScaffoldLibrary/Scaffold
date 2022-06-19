using Scaffold.Launcher.PackageHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Runners
{
    internal class LauncherValidation
    {
        private const string ValidationKey = "LASTVALIDATION";
        private const string InstallKey = "LASTINSTALL";

        [InitializeOnLoadMethod]
        private static void ValidateProject()
        {
            bool lastInstall = GetKey(InstallKey, true);
            bool lastValidation = GetKey(ValidationKey, false);
            if (lastInstall == lastValidation)
            {
                return;
            }

            if (CheckProjectDependencies())
            {
                SetKey(ValidationKey, lastInstall);
            }
        }

        [MenuItem("Scaffold/Launcher/Validate Dependencies")]
        private static bool CheckProjectDependencies()
        {
            ScaffoldManifest scaffoldManifest = ScaffoldManifest.Fetch();
            ProjectManifest projectManifest = ProjectManifest.Fetch();

            DependencyValidator validator = new DependencyValidator(scaffoldManifest, projectManifest);
            bool dependencyState = validator.ValidateDependencies();
            if (dependencyState)
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
