using Newtonsoft.Json;
using Scaffold.Builder.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


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
        public List<string> Assemblies = new List<string>(); //The list of assemblies used in the project
        public ModuleManifest Manifest
        {
            get
            {
                if (_manifest == null) CreateManifest();
                return _manifest;
            }
        } //Object representing the module manifest
        [SerializeField]
        private ModuleManifest _manifest;
        public Credentials Credential
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
        } //Object representing the uploading credentials
        public List<string> Dependencies
        {
            get
            {
                return Manifest.GetRawValue<List<string>>("requirements");
            }
            set
            {
                Manifest.SetValue("requirements", value);
            }
        } //List of required dependencies for package
        public List<string> RequiredDefines
        {
            get
            {
                return Manifest.GetRawValue<List<string>>("requiredDefines");
            }

            set
            {
                Manifest.SetValue("requiredDefines", value);
            }
        } //List of required Defines to install the package
        public List<string> InstallDefines
        {
            get
            {
                return Manifest.GetRawValue<List<string>>("installDefines");
            }
            set
            {
                Manifest.SetValue("installDefines", value);
            }
        } //list of defines added when you install the package
        public string ModuleName => GetNameAsPascal(); //module key formatted to PascalCase
        public string ModuleDefine => GetNameAsDefine(); //module key formatted to USE_SCAFFOLD_KEY

        //METHODS
        private void CreateManifest()
        {
            string rawManifest;
            if (string.IsNullOrEmpty(ManifestPath) || !File.Exists(ManifestPath))
            {
                rawManifest = TemplateManifest;
            }
            else
            {
                rawManifest = File.ReadAllText(ManifestPath);
            }
            _manifest = new ModuleManifest(rawManifest);
        }

        public void SetModuleFolder(string path)
        {
            ModuleFolder = path;
            ManifestPath = $"{path}/package.json";

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void ResetManifest()
        {
            CreateManifest();
            Debug.Log("Manifest Object Recreated!");
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

        //HELPERS
        private string GetNameAsPascal()
        {
            string name = Manifest.GetValue("name");
            return NameFormatter.KeyToPascal(name);
        }
        private string GetNameAsDefine()
        {
            string name = Manifest.GetValue("name");
            return NameFormatter.KeyToDefine(name);
        }

    }
}
