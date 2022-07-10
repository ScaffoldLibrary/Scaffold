using UnityEditor;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.Linq;
using Scaffold.Launcher.Editor;
using Scaffold.Launcher.Workers;
using UnityEditor.PackageManager;

namespace Scaffold.Launcher.Runners
{
#if !USE_SCAFFOLD_LAUNCHER || !USE_SCAFFOLD_CORE
    internal class LauncherInstaller
    {
        private const string CorePath = "https://github.com/MgCohen/Scaffold.git?path=/Assets/Scaffold/Core";
        private const string LauncherDefine = "USE_SCAFFOLD_LAUNCHER";

        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
#if !USE_SCAFFOLD_CORE
            InstallCore();
#endif

#if !USE_SCAFFOLD_LAUNCHER && USE_SCAFFOLD_CORE
            if (!IsLauncherInstalled())
            {
                TryInstallLauncherDefine();
            }
#endif
        }

        private static void InstallCore()
        {
            Client.Add(CorePath);
            ProjectDefines.AddDefines("USE_SCAFFOLD_CORE");
        }

        private static void TryInstallLauncherDefine()
        {
            if (IsLauncherInstalled())
            {
                return;
            }

            ProjectDefines.AddDefines(LauncherDefine);
            ScaffoldLauncher.Launch();
        }

        private static bool IsLauncherInstalled()
        {
            return ProjectDefines.CheckDefines(LauncherDefine);
        }
    }
#endif
}

