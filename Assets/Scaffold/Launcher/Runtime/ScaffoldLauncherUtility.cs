using UnityEditor;

namespace Scaffold.Core.Launcher
{
    internal class ScaffoldLauncherUtility
    {
        private static string InitKey = "LauncherInitialized";
        private static string InstallKey = "LauncherInstalled";
        public static bool IsLauncherInitialized
        {
            get
            {
                return EditorPrefs.GetBool(InitKey, false);
            }
        }

        public static bool IsLauncherInstalled
        {
            get
            {
                return EditorPrefs.GetBool(InstallKey, false);
            }
        }

        public static void SetLauncherInitialized(bool state)
        {
            EditorPrefs.SetBool(InitKey, state);
        }

        public static void SetLauncherInstalled(bool state)
        {
            EditorPrefs.SetBool(InstallKey, state);
        }
    }
}