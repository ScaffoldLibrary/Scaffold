using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

namespace Scaffold.Builder.Installer
{
#if !USE_SCAFFOLD_BUILDER || !USE_SCAFFOLD_CORE
    internal class LauncherInstaller
    {
        private const string CorePath = "https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Core";
        private const string BuilderDefine = "USE_SCAFFOLD_BUILDER";
        private const string CoreDefine = "USE_SCAFFOLD_CORE";

        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
#if !USE_SCAFFOLD_CORE
            InstallCore();
#endif

#if !USE_SCAFFOLD_BUILDER && USE_SCAFFOLD_CORE
            InstallBuilder();
#endif
        }

        private static void InstallCore()
        {
            AddDefine(CoreDefine);
            Client.Add(CorePath);
        }

        private static void InstallBuilder()
        {
            AddDefine(BuilderDefine);
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

