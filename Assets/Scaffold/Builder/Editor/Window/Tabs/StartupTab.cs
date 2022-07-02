using Scaffold.Builder.Utilities;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(0)]
    public class StartupTab : WindowTab
    {
        public StartupTab(BuilderWindow window, BuilderConfigs config) : base(window, config)
        {
            _modulePath = config.ModuleFolder;
        }

        public override string TabKey => "Setting up module...";

        private string _modulePath;
        private bool _createdManifest;
        private bool _hasManifest;

        public override void Draw()
        {
            EditorGUILayout.LabelField("Welcome to the Scaffold Builder Wizard, let's build your module!");
            EditorGUILayout.Space(10);

            //Project Folder
            EditorGUILayout.LabelField("Module Folder:", EditorStyles.CornerLabel);
            EditorGUILayout.BeginHorizontal();
            {
                GUIContent icon =  _hasManifest ? EditorGUIUtility.IconContent("greenLight") : EditorGUIUtility.IconContent("redLight");
                EditorGUILayout.LabelField(icon, GUILayout.Width(20));
                _modulePath = EditorGUILayout.TextField(_modulePath);
                if (GUILayout.Button("Select Folder"))
                {
                    string path = EditorUtility.OpenFolderPanel("Select module folder", "", "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        _modulePath = path;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (string.IsNullOrEmpty(_modulePath))
            {
                return;
            }

            EditorGUILayout.Space(15);
            _hasManifest = Directory.Exists(_modulePath) &&  Directory.GetFiles(_modulePath, "package.json", SearchOption.TopDirectoryOnly).Length > 0;

            if (!_hasManifest)
            {
                EditorGUILayout.HelpBox("There is no package manifest on the selected folder, do you wish to create one?", MessageType.Warning);
                EditorGUILayout.Space(5);
                if (GUILayout.Button("Create Manifest"))
                {
                    _configs.SetModuleFolder(_modulePath);
                    ScaffoldBuilder.CreateManifest();
                    _createdManifest = true;
                }
                return;
            }

            if (_hasManifest && _createdManifest)
            {
                EditorGUILayout.HelpBox("Manifest Created!", MessageType.Info);
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.LabelField("Follow the next few steps to setup your module.\nPress Next when you are ready.", GUILayout.Height(30));
        }

        public override void OnNext()
        {
            _configs.SetModuleFolder(_modulePath);
        }

        public override bool ValidateNext()
        {
            if (string.IsNullOrEmpty(_modulePath))
            {
                return false;
            }

            
            return _hasManifest;
        }
    }
}