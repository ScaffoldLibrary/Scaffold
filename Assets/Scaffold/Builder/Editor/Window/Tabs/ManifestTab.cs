using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Core.Editor;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(1)]
    public class ManifestTab : WindowTab
    {
        public ManifestTab(BuilderWindow window, BuilderConfigs config) : base(window, config)
        {
            _manifest = config.Manifest;
        }

        private ModuleManifest _manifest;

        public override string TabKey => "Creating Manifest...";

        public override void Draw()
        {
            List<string> props = _manifest.GetProps();
            foreach (string prop in props)
            {
                object value = _manifest.GetRawValue(prop);
                if (value is JObject obj)
                {
                    DrawObject(prop, obj);
                }
                else
                if (value is JArray array)
                {
                    //TODO: Create a array visualization
                    continue;
                }
                else
                {
                    string text = _manifest.GetValue(prop);
                    DrawField(prop, text);
                }

            }
        }

        private void DrawField(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"{label}: ", ScaffoldStyles.CornerLabel, GUILayout.MaxWidth(100));
                value = EditorGUILayout.TextField(value, ScaffoldStyles.TextField);
                _manifest.SetValue(label, value);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }

        private void DrawObject(string label, JObject token)
        {
            if (token.Count <= 0)
            {
                return;
            }

            EditorGUILayout.LabelField($"{label}: ", ScaffoldStyles.CornerLabel);
            Dictionary<string, string> objectInput = new Dictionary<string, string>();
            EditorGUILayout.BeginVertical();
            {
                foreach (var entry in token)
                {
                    Rect rect = EditorGUILayout.BeginHorizontal();
                    {
                        rect.height = 18;
                        string name = entry.Key;
                        string value = entry.Value.ToString();

                        Rect labelRect = new Rect(rect);
                        labelRect.x += 20;
                        labelRect.width = 100;
                        EditorGUI.LabelField(labelRect, $"{name}: ");

                        Rect fieldRect = new Rect(rect);
                        fieldRect.x = 105;
                        fieldRect.width -= 110;
                        string result = EditorGUI.TextField(fieldRect, value);
                        objectInput.Add(name, result);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(20);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);

            JObject content = JObject.FromObject(objectInput);
            _manifest.SetValue(label, content);
        }

        public override void OnNext()
        {
            ManifestReader reader = new ManifestReader("");
            Debug.LogError("FAKE READER EXPOSED");
            List<string> dependencies = reader.GetModuleDependencies();
            _configs.Dependencies = dependencies;
            List<string> requiredDefines = ConvertDependenciesToDefines(dependencies);
            _configs.RequiredDefines = requiredDefines;
            List<string> installDefines = new List<string>() { _configs.ModuleDefine };
            _configs.InstallDefines = installDefines;
            _manifest.Save(_configs.ManifestPath);
        }

        private List<string> ConvertDependenciesToDefines(List<string> dependencies)
        {
            return dependencies.Select(d => FormatName(d))
                               .ToList();
        }

        private string FormatName(string name)
        {
            return NameFormatter.KeyToDefine(name);
        }

        public override bool ValidateNext()
        {
            return _configs.Manifest.Validate();
        }
    }
}