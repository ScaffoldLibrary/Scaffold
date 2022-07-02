using Newtonsoft.Json;
using Scaffold.Builder.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor.Module;


namespace Scaffold.Builder
{
    [CreateAssetMenu(menuName = "Scaffold/Builder/Create config file")]
    public class BuilderConfigs : ScriptableObject
    {
        //TEMPLATES
        [HideInInspector]
        public string TemplateAssembly = string.Empty; //Template content for assemblies
        [HideInInspector]
        public string TemplateInstaller = string.Empty; //Template content for installers
        [HideInInspector]
        public string TemplateManifest = string.Empty; //Template content for manifest

        //PATHS
        [HideInInspector]
        public string ManifestPath = string.Empty; //The module manifest file path
        [HideInInspector]
        public string ModuleFolder = string.Empty; //The main package folder, used to find Manifest
        [HideInInspector]
        public string CredentialsPath = string.Empty; //The credentials path stored outside of project
        [HideInInspector]
        public string InstallerPath = string.Empty; //The installer path for quick building
        [HideInInspector]
        public string InstallerAssemblyPath = string.Empty; //The Installer Assembly Path for quick building

        //VALUES
        [HideInInspector]
        public Module Module;
        [HideInInspector]
        public List<string> Assemblies = new List<string>(); //The list of assemblies used in the project
        public Credentials Credential //Object representing the uploading credentials
        {
            get
            {
                if (!File.Exists(CredentialsPath))
                {
                    return null;
                }
                string rawCredentials = File.ReadAllText(CredentialsPath);
                return JsonConvert.DeserializeObject<Credentials>(rawCredentials);
            }
        }

     
        //METHODS
        public void SetModuleFolder(string path)
        {
            ModuleFolder = path;
            ManifestPath = $"{path}/package.json";

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        //LOCAL SETTERS - Used on the main builder project only
        public void SetTemplateAssembly(string assembly)
        {
            TemplateAssembly = assembly;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void SetTemplateInstaller(string installer)
        {
            TemplateInstaller = installer;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void SetTemplateManifest(string manifest)
        {
            TemplateManifest = manifest;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
