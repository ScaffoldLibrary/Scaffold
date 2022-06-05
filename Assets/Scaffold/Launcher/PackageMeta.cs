using System.Collections.Generic;

public class PackageManifest
{
    public List<string> dependencies;
}

[System.Serializable]
public class PackagePath
{
    public PackagePath(string packageName, string packageReference, string packagePath)
    {
        name = packageName;
        reference = packageReference;
        path = packagePath;
    }

    public PackagePath(string packageReference, string packagePath)
    {
        reference = packageReference;
        path = packagePath;
    }


    public string name;
    public string reference;
    public string path;
}