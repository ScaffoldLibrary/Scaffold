using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Scaffold.Core.Editor
{
    public static class HttpHandler
    {
        public static async Task<string> Post(string endpoint, object payload)
        {
            var uwr = await Post_Internal(endpoint, payload);

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                return "Upload Failed";
            }
            else
            {
                
                return "Upload Successful";
            }
        }

        public static async Task<T> Post<T>(string endpoint, object payload)
        {
            var uwr = await Post_Internal(endpoint, payload);

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                return default(T);
            }
            else
            {
                JsonConvert.SerializeObject(uwr.downloadHandler.text, Formatting.Indented);
                T obj = JsonConvert.DeserializeObject<T>(uwr.downloadHandler.text);
                return obj;
            }
        }

        private static async Task<UnityWebRequest> Post_Internal(string endpoint, object payload)
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

            return uwr;
        }
    }
}