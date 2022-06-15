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
            bool lastInstall = GetKey(InstallKey);
            bool lastValidation = GetKey(ValidationKey);
            if(lastInstall == lastValidation)
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
            PackageValidator validator = new PackageValidator();
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

        private static bool GetKey(string key)
        {
            return PlayerPrefs.GetInt(key) == 1;
        }

        private static void SetKey(string key, bool state)
        {
            int value = state ? 1 : 0;
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }
}