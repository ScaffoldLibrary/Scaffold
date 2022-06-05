using Scaffold.Core.Launcher;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher
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

