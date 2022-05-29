using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor;

public class PackageInstaller
{
    [MenuItem("Tools/Package")]
    public static void Install()
    {
        Client.Add("ssh://git@bitbucket.org/projectviker/vikerpackagemanager.git?path=Assets/VikerCommon");
    }
}
