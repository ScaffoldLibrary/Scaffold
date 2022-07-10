using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using UnityEngine;
using System;

#if !USE_SCAFFOLD_BUILDER
namespace Scaffold.Builder.Installer
{
    internal class BuilderInstaller : EditorWindow
    {
        private const string Module = "BUILDER";
        private static string ModuleDefine => $"USE_SCAFFOLD_{Module}";
        private static string LauncherDefine => $"USE_SCAFFOLD_LAUNCHER";
        private static string LauncherSkipKey => $"LAUNCHERSKIP{Module}";
        private static string InstallKey = "LASTINSTALL";

        private static readonly string[] RequiredModules = {  };

        private static List<string> RequiredDefines
        {
            get
            {
                if(_requiredDefines == null)
                {
                    _requiredDefines = GetRequiredDefines();
                }
                return _requiredDefines;
            }
        }
        private static List<string> _requiredDefines;
        private static List<string> ProjectDefines
        {
            get
            {
                if (_projectDefines == null)
                {
                    _projectDefines = GetProjectDefines();
                }
                return _projectDefines;
            }
            set
            {
                _projectDefines = value;
            }
        }
        private static List<string> _projectDefines;

        [InitializeOnLoadMethod]
        private static void ValidatePackage()
        {
            bool isPackageInstalled = ProjectDefines.Contains(ModuleDefine);
            if (isPackageInstalled)
            {
                return;
            }

            InstallModuleDefines();
            bool hasRequiredDefines = !RequiredDefines.Except(ProjectDefines).Any();
            if (!hasRequiredDefines)
            {
                RequestLauncher();
            }
        }

        private static void RequestLauncher()
        {
            if (ProjectDefines.Contains(LauncherDefine))
            {
                return;
            }
            else
            {
                if (GetKey(LauncherSkipKey))
                {
                    Debug.Log("Player already decided to skip launcher install from this module");
                    return;
                }
                TryInstallLauncher();
            }
        }

#if !USE_SCAFFOLD_LAUNCHER
        [MenuItem("Scaffold/Launcher/Install Launcher")]
#endif
        private static void TryInstallLauncher()
        {
            if (ProjectDefines.Contains(LauncherDefine))
            {
                Debug.Log("Launcher already installed!");
                return;
            }

            OpenInstallPopup();
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

        private static void InstallModuleDefines()
        {
            if (ProjectDefines.Contains(ModuleDefine))
            {
                return;
            }

            ProjectDefines.Add(ModuleDefine);

            bool installState = GetKey(InstallKey);
            SetKey(InstallKey, !installState);

            string defineString = string.Join(";", ProjectDefines.ToArray());
            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defineString);
        }

        private static List<string> GetProjectDefines()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines;
        }

        private static List<string> GetRequiredDefines()
        {
            List<string> defines = new List<string>();
            foreach(string module in RequiredModules)
            {
                defines.Add($"USE_SCAFFOLD_{module.ToUpperInvariant()}");
            }
            return defines;
        }

        private static bool GetKey(string key)
        {
            return PlayerPrefs.GetInt(key, 0) == 1 ? true : false;
        }

        private static void SetKey(string key, bool value)
        {
            int boolean = value ? 1 : 0;
            PlayerPrefs.SetInt(key, boolean);
        }

        private static void OpenInstallPopup()
        {
            BuilderInstaller window = ScriptableObject.CreateInstance(typeof(BuilderInstaller)) as BuilderInstaller;
            window.ShowModalUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("This package needs the following modules:");
            foreach(string dependency in RequiredModules)
            {
                EditorGUILayout.LabelField($"- {dependency}");
            }
            EditorGUILayout.LabelField("Do you wish to install the Scaffold Launcher to handle the missing modules?");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("No"))
            {
                SetKey(LauncherSkipKey, true);
                Close();
            }
            if (GUILayout.Button("Yes"))
            {
                Close();
                InstallLauncher();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
