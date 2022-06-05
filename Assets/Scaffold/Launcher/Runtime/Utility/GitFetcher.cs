using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Scaffold.Launcher.Utilities
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
                Debug.Log(text);
                if(typeof(T) == typeof(string))
                {
                    Debug.Log(1);
                    T convertedValue = (T)Convert.ChangeType(text, typeof(T));
                    Debug.Log(2);
                    callback?.Invoke(convertedValue);
                    return;
                }
                Debug.Log(3);
                Debug.Log(text);
                T content = JsonConvert.DeserializeObject<T>(text);
                Debug.Log(content);
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