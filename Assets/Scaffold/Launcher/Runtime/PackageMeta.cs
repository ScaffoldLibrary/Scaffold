using System.Collections.Generic;

public class PackageManifest
{
    public Dictionary<string, string> dependencies;
}

[System.Serializable]
public class PackagePath
{
    public PackagePath(string packageName, string packageKey, string packagePath)
    {
        name = packageName;
        key = packageKey;
        path = packagePath;
    }

    public PackagePath(string packageKey, string packagePath)
    {
        key = packageKey;
        path = packagePath;
    }


    public string name;
    public string key;
    public string path;

    public string manifestPath;
    public List<string> dependencies = new List<string>();
}