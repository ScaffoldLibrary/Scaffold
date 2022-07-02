using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Builder;
using Scaffold.Builder.Editor;
using UnityEditor;
using System.IO;

public class ConfigSetter
{
    [MenuItem("Scaffold/Builder/Config/Set Assembly Template")]
    public static void UpdateAssemblyTemplate()
    {
        string assembly = EditorUtility.OpenFilePanel("Select Assembly Template", "", "");
        string content = File.ReadAllText(assembly);
        ScaffoldBuilder.Config.SetTemplateAssembly(content);
    }

    [MenuItem("Scaffold/Builder/Config/Set Manifest Template")]
    public static void UpdateManifestTemplate()
    {
        string manifest = EditorUtility.OpenFilePanel("Select Manifest Template", "", "");
        string content = File.ReadAllText(manifest);
        ScaffoldBuilder.Config.SetTemplateManifest(content);
    }

    [MenuItem("Scaffold/Builder/Config/Set Installer Template")]
    public static void UpdateInstallerTemplate()
    {
        string installer = EditorUtility.OpenFilePanel("Select Installer Template", "", "");
        string content = File.ReadAllText(installer);
        ScaffoldBuilder.Config.SetTemplateInstaller(content);
    }
}
