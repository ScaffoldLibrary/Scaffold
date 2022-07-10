using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using UnityEngine;
using System;

#if _MODULEDEFINES_
namespace Scaffold.ModuleName.Installer
{
    internal class ModuleNameInstaller
    {
        private static string LauncherDefine = "USE_SCAFFOLD_LAUNCHER";
        private static string LauncherSkipKey = "LAUNCHERSKIPMODULENAME";
        private static string ValidationKey = "PROJECTVALIDATED";

        private static readonly string[] RequiredDefines = { "#REQUIREMENTS#" };
        private static readonly string[] InstallDefines = { "#INSTALLS#" };

        [InitializeOnLoadMethod]
        private static void ValidatePackage()
        {
            List<string> Defines = GetProjectDefines();

            bool hasRequiredDefines = !RequiredDefines.Except(Defines).Any();
            if (!hasRequiredDefines)
            {
                RequestLauncher();
                return;
            }

            bool isPackageInstalled = !InstallDefines.Except(Defines).Any();
            if (isPackageInstalled)
            {
                return;
            }
            InstallModuleDefines(Defines);
        }

        private static void RequestLauncher()
        {
            if (GetKey(LauncherSkipKey))
            {
                return;
            }
            TryInstallLauncher();
        }

#if !USE_SCAFFOLD_LAUNCHER
        [MenuItem("Scaffold/Launcher/Install Launcher")]
#endif
        private static void TryInstallLauncher()
        {
            List<string> Defines = GetProjectDefines();
            if (Defines.Contains(LauncherDefine))
            {
                Debug.Log("Launcher already installed!");
                return;
            }

            OpenInstallPopup();
        }

        private static void OpenInstallPopup()
        {
            string title = "Missing dependencies";
            string description = "This project requires uninstalled modules, do you wish to install the Scaffold Launcher and resolve?";
            string confirm = "yes";
            string cancel = "no";
            bool install = EditorUtility.DisplayDialog(title, description, confirm, cancel);
            if (!install)
            {
                SetKey(LauncherSkipKey, true);
            }
            else
            {
                InstallLauncher();
            }
        }

        private static async void InstallLauncher()
        {
            AddRequest add = Client.Add("https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Launcher");

            while (!add.IsCompleted)
            {
                await Task.Delay(100);
            }

            if (add.Status != StatusCode.Success)
            {
                Debug.LogError("Launcher installation failed, please try again");
            }
        }

        private static void InstallModuleDefines(List<string> currentDefines)
        {
            foreach (string define in InstallDefines)
            {
                if (!currentDefines.Contains(define))
                {
                    currentDefines.Add(define);
                }
            }

            SetKey(ValidationKey, false);

            string defineString = string.Join(";", currentDefines.ToArray());
            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defineString);
        }

        private static List<string> GetProjectDefines()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines;
        }

        private static bool GetKey(string key)
        {
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        private static void SetKey(string key, bool value)
        {
            int boolean = value ? 1 : 0;
            PlayerPrefs.SetInt(key, boolean);
        }
    }
}
#endif
