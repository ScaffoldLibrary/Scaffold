using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Scaffold.Core.Launcher.Utilities
{
    internal class GitFetcher
    {
        public static void Fetch<T>(string uri, Action onRequestStarted = null, Action<T> onRequestCompleted = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            onRequestStarted?.Invoke();
            asyncOperation.completed += (op) => {Resolve(webRequest, onRequestCompleted); };
        }

        private static void Resolve<T>(UnityWebRequest request, Action<T> callback)
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string text = request.downloadHandler.text;
                T content = JsonConvert.DeserializeObject<T>(text);
                callback?.Invoke(content);
            }
            else
            {
                string[] pages = request.url.Split('/');
                int page = pages.Length - 1;
                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + request.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + request.error);
                        break;
                }

                callback?.Invoke(default(T));
            }
        }
    }
}