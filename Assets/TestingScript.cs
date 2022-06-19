using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Scaffold.Launcher.Requests;
using Scaffold.Launcher.PackageHandler;

public class TestingScript : MonoBehaviour
{
    [ContextMenu("Test")]
    public void Test()
    {
        StartCoroutine(PostRequest<ScaffoldManifest>("https://e227iwvnp2dov6ddpl3ujveyyi0vahcu.lambda-url.us-east-1.on.aws/", new GetAllModulesRequest()));
    }

    private IEnumerator PostRequest<T>(string endpoint, object payload)
    {
        string stringifiedPayload = JsonConvert.SerializeObject(payload);

        var uwr = new UnityWebRequest(endpoint, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(stringifiedPayload);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            JsonConvert.SerializeObject(uwr.downloadHandler.text, Formatting.Indented);
        }

    }

    private class ErrorMessage
    {
        public string message;
    }

    private class Payload
    {
        public int point = 3;
    }
}
