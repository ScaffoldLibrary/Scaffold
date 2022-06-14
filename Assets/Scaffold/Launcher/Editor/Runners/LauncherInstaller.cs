using UnityEditor;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.Linq;
using Scaffold.Launcher.Editor;

namespace Scaffold.Launcher.Runners
{
#if !USE_SCAFFOLD_LAUNCHER
    internal class LauncherInstaller
    {
        private static List<string> ProjectDefines = new List<string>();
        private static string LauncherDefine = "USE_SCAFFOLD_LAUNCHER";

        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
            ProjectDefines = GetProjectDefines();
            if (!IsLauncherInstalled())
            {
                TryInstallLauncherDefine();
            }
        }

        private static List<string> GetProjectDefines()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines;
        }

        private static void TryInstallLauncherDefine()
        {
            if (IsLauncherInstalled())
            {
                return;
            }

            ProjectDefines.Add(LauncherDefine);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", ProjectDefines.ToArray()));

            LauncherWindow.OpenLauncher();
        }

        private static bool IsLauncherInstalled()
        {
            return ProjectDefines.Contains(LauncherDefine);
        }
    }
#endif
}

