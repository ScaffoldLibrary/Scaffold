using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Scaffold.Builder.Utilities
{
    internal static class HttpFetcher
    {
        public static async Task<string> Fetch(string endpoint, object payload)
        {
            string stringifiedPayload = JsonConvert.SerializeObject(payload);

            var uwr = new UnityWebRequest(endpoint, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(stringifiedPayload);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Delay(100);
            }

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                return "Upload Failed";
            }
            else
            {
                
                return "Upload Successful";
            }
        }
    }
}