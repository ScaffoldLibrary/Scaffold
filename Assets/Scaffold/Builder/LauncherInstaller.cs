using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Scaffold.Builder.Installer
{
    internal class LauncherInstaller
    {
        private static AddRequest ClientRequest;

        [InitializeOnLoadMethod]
        private async static void RequestLauncher()
        {
            await Task.Delay(1000);
            if (!EditorPrefs.GetBool("LauncherInstalled"))
            {
                ClientRequest = Client.Add("https://github.com/MgCohen/Scaffold-Launcher.git?path=/Assets/Scaffold/Launcher");
            }
        }

        public static async Task<bool> WaitForClient()
        {
            while (!ClientRequest.IsCompleted)
            {
                await Task.Delay(1000);
            }

            return ClientRequest.Status == StatusCode.Success;
        }
    }
}

