using UnityEditor;

namespace Scaffold.Core.Launcher
{
    internal class ScaffoldLauncherUtility
    {
        private static string InitKey = "LauncherInitialized";
        public static bool IsLauncherInitialized
        {
            get
            {
                return EditorPrefs.GetBool(InitKey, false);
            }
        }

        public static void SetLauncherInitialied(bool state)
        {
            EditorPrefs.SetBool(InitKey, state);
        }
    }
}