using System.Collections;
using UnityEngine;
using Scaffold.Launcher.Objects;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

namespace Scaffold.Launcher.Utilities
{
    internal static class HttpFetcher
    {
        public static async Task<T> Fetch<T>(string endpoint, object payload)
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

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
                return default(T);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
                JsonConvert.SerializeObject(uwr.downloadHandler.text, Formatting.Indented);
                T obj = JsonConvert.DeserializeObject<T>(uwr.downloadHandler.text);
                return obj;
            }
        }
    }
}