using Newtonsoft.Json;
using Scaffold.Builder.Editor.Components;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(4)]
    public class UploadTab : WindowTab
    {
        public UploadTab(BuilderWindow window, BuilderConfigs config) : base(window, config)
        {

        }

        public override string TabKey => "Uploading module to server";

        //credential Paths
        private string _credentialsPath;
        private bool _hasCredentials;

        //uploading state
        private bool _uploading;
        private string _returnMessage;

        public override void Draw()
        {
            EditorGUILayout.LabelField("Package ready to upload! Check your credentials to continue:");
            EditorGUILayout.Space(10);
            _hasCredentials = CheckCredentials(out Credentials credentials);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    string iconName = _hasCredentials ? "greenLight" : "redLight";
                    var icon = EditorGUIUtility.IconContent(iconName);
                    icon.text = "  Credentials: ";
                    EditorGUILayout.LabelField(icon, GUILayout.Width(100));

                    string fileName = !string.IsNullOrWhiteSpace(_credentialsPath) ? Path.GetFileName(_credentialsPath) : "";
                    EditorGUILayout.TextField(fileName);
                }
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Search"))
                {
                    string path = EditorUtility.OpenFilePanel("Select credentials file", "", "");
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return;
                    }
                    _credentialsPath = path;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (!_hasCredentials)
            {
                return;
            }

            EditorGUILayout.Space(5);

            
            if (!_uploading)
            {
                if (GUILayout.Button("Upload Module", GUILayout.Width(150)))
                {
                    if (string.IsNullOrWhiteSpace(_configs.CredentialsPath))
                    {
                        _configs.CredentialsPath = _credentialsPath;
                    }
                    _uploading = true;
                    _returnMessage = "";
                    ModuleUploader.UploadModule(_configs, (s) => { _returnMessage = s; _uploading = false; });
                }

                EditorGUILayout.LabelField(_returnMessage);
                return;
            }

            EditorGUI.BeginDisabledGroup(true);
            {
                GUILayout.Button("Uploading...");
            }
            EditorGUI.EndDisabledGroup();
        }


        private bool CheckCredentials(out Credentials credentials)
        {
            credentials = null;
            if (string.IsNullOrWhiteSpace(_credentialsPath))
            {
                return false;
            }

            if (!File.Exists(_credentialsPath))
            {
                return false;
            }

            string rawCredentials = File.ReadAllText(_credentialsPath);
            credentials = JsonConvert.DeserializeObject<Credentials>(rawCredentials);
            return true;
        }

        public override void OnNext()
        {
            if (_hasCredentials)
            {
                _configs.CredentialsPath = _credentialsPath;
            }
        }

        public override bool ValidateNext()
        {
            return true;
        }
    }
}