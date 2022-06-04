using System.Collections.Generic;

public class PackageManifest
{
    public List<string> dependencies;
}

[System.Serializable]
public class PackagePath
{
    public PackagePath(string packageName, string packagePath)
    {
        name = packageName;
        path = packagePath;
    }

    public string name;
    public string path;
}