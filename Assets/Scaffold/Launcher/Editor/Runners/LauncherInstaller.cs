using UnityEditor;
using Scaffold.Launcher.Utilities;
using System.Collections.Generic;
using System.Linq;
using Scaffold.Launcher.Editor;
using Scaffold.Launcher.PackageHandler;

namespace Scaffold.Launcher.Runners
{
#if !USE_SCAFFOLD_LAUNCHER
    internal class LauncherInstaller
    {
        private static string LauncherDefine = "USE_SCAFFOLD_LAUNCHER";

        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
            if (!IsLauncherInstalled())
            {
                TryInstallLauncherDefine();
            }
        }

        private static void TryInstallLauncherDefine()
        {
            if (IsLauncherInstalled())
            {
                return;
            }

            DefinesHandler.AddDefines(LauncherDefine);
            ScaffoldLauncher.Launch();
        }

        private static bool IsLauncherInstalled()
        {
            return DefinesHandler.CheckDefines(LauncherDefine);
        }
    }
#endif
}

