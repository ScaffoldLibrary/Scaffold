using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

namespace Scaffold.Launcher.Installer
{
#if !USE_SCAFFOLD_LAUNCHER || !USE_SCAFFOLD_CORE
    internal class LauncherInstaller
    {
        private const string CorePath = "https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Core";
        private const string LauncherDefine = "USE_SCAFFOLD_LAUNCHER";
        private const string CoreDefine = "USE_SCAFFOLD_CORE";

        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
#if !USE_SCAFFOLD_CORE
            InstallCore();
#endif

#if !USE_SCAFFOLD_LAUNCHER && USE_SCAFFOLD_CORE
            InstallLauncher();
#endif
        }

        private static void InstallCore()
        {
            AddDefine(CoreDefine);
            Client.Add(CorePath);
        }

        private static void InstallLauncher()
        {
            AddDefine(LauncherDefine);
        }

        private static void AddDefine(string define)
        {
            List<string> defines = GetProjectDefines();
            if (defines.Contains(define))
            {
                return;
            }
            defines.Add(define);
            string defineString = string.Join(";", defines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineString);
        }

        private static List<string> GetProjectDefines()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines;
        }
    }
#endif
}

