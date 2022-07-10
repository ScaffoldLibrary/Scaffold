using Scaffold.Builder.Utilities;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(0)]
    public class StartupTab : WindowTab
    {
        public StartupTab(BuilderConfigs config) : base(config)
        {
            _moduleManifest = config.ModuleManifestPath;
            _projectManifest = config.ProjectManifestPath;
            _credentials = config.CredentialsPath;
        }

        public override string TabKey => "Setting up module...";

        private string _moduleManifest;
        private string _projectManifest;
        private string _credentials;

        private string _defaultProjectManifestPath = "./Packages/manifest.json";

        public override void OnDraw()
        {
            if (string.IsNullOrWhiteSpace(_projectManifest))
            {
                _projectManifest = _defaultProjectManifestPath;
            }
        }

        public override void Draw()
        {
            EditorGUILayout.LabelField("Welcome to the Scaffold Builder Wizard, let's build your module!", ScaffoldStyles.WrappedLabel);
            EditorGUILayout.LabelField("Please, provide the path to the Project Manifest and the Package Manifest", ScaffoldStyles.WrappedLabel);
            EditorGUILayout.Space(10);

            bool hasModule = File.Exists(_moduleManifest);
            _moduleManifest = ScaffoldComponents.FileSearchOrCreate(_moduleManifest, "Package Manifest: ", hasModule, "./Assets", "json", CreateModuleManifest);
            bool hasProject = File.Exists(_projectManifest);
            _projectManifest = ScaffoldComponents.FileField(_projectManifest, "Project Manifest: ", "./", "json", hasProject);
            bool hasCredentials = File.Exists(_credentials);
            _credentials = ScaffoldComponents.FileField(_credentials, "Credentials: ", "./", "json", hasCredentials);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Follow the next few steps to setup your module.\nPress Next when you are ready.", ScaffoldStyles.WrappedLabel);
        }

        private void CreateModuleManifest()
        {
            string path = EditorUtility.OpenFolderPanel("Select manifest folder", "./Assets", "");
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            path = $"{path}/package.json";
            string templateManifest = _configs.TemplateManifest;
            FileService files = new FileService();
            files.Save(templateManifest, path);
            _moduleManifest = path;
        }

        public override void OnNext()
        {
            _configs.ModuleManifestPath = _moduleManifest;
            _configs.ProjectManifestPath = _projectManifest;
            _configs.CredentialsPath = _credentials;


            FileService file = new FileService();
            _configs.Module = file.Read<Module>(_configs.ModuleManifestPath);
        }

        public override bool ValidateNext()
        {
            if (string.IsNullOrWhiteSpace(_moduleManifest))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_projectManifest))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_credentials))
            {
                return false;
            }

            return true;
        }
    }
}