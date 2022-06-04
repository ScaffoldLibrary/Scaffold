using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FileModifier : MonoBehaviour
{
    public TextAsset editableFile;

    [SerializeField] private List<PackagePath> paths = new List<PackagePath>();

    [ContextMenu("Read")]
    public string ReadFile()
    {
        string text = editableFile.text;
        Debug.Log(text);
        return text;
    }

    private string ReadManifest()
    {
        var path = "./Packages/manifest.json";
        var value = File.ReadAllText(path);
        Debug.Log(value);
        return value;
    }


    [ContextMenu("Convert")]
    public void Convert()
    {
        var parsedFile = JObject.Parse(ReadFile());
        Debug.Log(parsedFile["dependencies"]);
        var dependencies = parsedFile["dependencies"].ToArray().Select(j => j.ToString()).ToList();
        Debug.Log(dependencies[0]);
    }

    [ContextMenu("Add")]
    public JObject Add()
    {
        var manifest = JObject.Parse(ReadFile());
        var dependencies = manifest["dependencies"];
        var package = paths[0];
        dependencies.First.AddBeforeSelf(new JProperty(package.name, package.path));
        Debug.Log(dependencies);
        Debug.Log(manifest);
        return manifest;
    }
    
    public JObject Add(string text)
    {
        var manifest = JObject.Parse(text);
        var dependencies = manifest["dependencies"];
        var package = paths[0];
        dependencies.First.AddBeforeSelf(new JProperty(package.name, package.path));
        Debug.Log(dependencies);
        Debug.Log(manifest);
        return manifest;
    }

    [ContextMenu("Write")]
    public void Write()
    {   
        var content = Add();
        var json = JsonConvert.SerializeObject(content, Formatting.Indented);
        EditTextAsset(json);
    }

    [ContextMenu("Write Manifest")]
    public void WriteManifest()
    {
        var text = ReadManifest();
        var content = Add(text);
        var json = JsonConvert.SerializeObject(content, Formatting.Indented);
        EditTextAsset(json);
    }

    [ContextMenu("Check")]
    public void CheckFor()
    {
        var manifest = JObject.Parse(ReadFile());
        var dependencies = manifest["dependencies"];
        Debug.Log(dependencies.Count());
        Debug.Log(dependencies["mything"] != null);
    }

    public void EditTextAsset(string text)
    {
        File.WriteAllText(AssetDatabase.GetAssetPath(editableFile), text);
        EditorUtility.SetDirty(editableFile);
    }

    public void EditManifest(string text)
    {
        File.WriteAllText("./Packages/manifest.json", text);
    }
}
