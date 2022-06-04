using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class FileFetcher : MonoBehaviour
{
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://github.com/MgCohen/Scaffold-Core/raw/main/Assets/Scaffold/Core/Events/Runtime/Signals/Signal.cs"));
        Try("https://github.com/MgCohen/Scaffold-Core/raw/main/Assets/Scaffold/Core/Events/Runtime/Signals/Signal.cs");
        //// A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + " Received:\n" + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    [ContextMenu("Try On Editor")]
    public void Try()
    {
        Try("https://github.com/MgCohen/Scaffold-Core/raw/main/Assets/Scaffold/Core/Events/Runtime/Signals/Signal.cs");
    }

    public void Try(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        UnityWebRequestAsyncOperation a = webRequest.SendWebRequest();
        Debug.Log("Starting");
        a.completed += (s) => Resolve(webRequest);
    }

    private void Resolve(UnityWebRequest request)
    {
        Debug.Log("completed");
        Debug.Log(request.result);
        Debug.Log(request.downloadHandler.text);
    }
}