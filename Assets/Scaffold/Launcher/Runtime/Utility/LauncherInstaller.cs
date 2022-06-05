using UnityEditor;
using Scaffold.Launcher.Utilities;

namespace Scaffold.Launcher.Installer
{
    internal class LauncherInstaller
    {
        [InitializeOnLoadMethod]
        private static void installLauncher()
        {
            ScaffoldLauncherUtility.SetLauncherInstalled(true);

            if (ScaffoldLauncherUtility.IsLauncherInitialized)
            {
                ScaffoldLauncherUtility.SetLauncherInitialized(true);
                ScaffoldLauncher launcher = new ScaffoldLauncher();
                launcher.Init();
            }
        }
    }
}

