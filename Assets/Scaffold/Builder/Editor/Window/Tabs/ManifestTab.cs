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
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Manifest;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(1)]
    public class ManifestTab : WindowTab
    {
        public ManifestTab(BuilderConfigs config) : base(config)
        {

        }

        private Module _module;

        public override string TabKey => "Creating Manifest...";
        public override void OnDraw()
        {
            _module = _configs.Module;
        }

        public override void Draw()
        {
            _module.name = EditorGUILayout.TextField("Name: ", _module.name);
            _module.displayName = EditorGUILayout.TextField("Display Name: ", _module.displayName);
            _module.path = EditorGUILayout.TextField("Path: ", _module.path);
            _module.unity = EditorGUILayout.TextField("Unity Version: ", _module.unity);
            _module.version = EditorGUILayout.TextField("Version: ", _module.version);

            EditorGUILayout.LabelField("Description: ");
            _module.description = EditorGUILayout.TextArea(_module.description, GUILayout.Height(65));
        }

        public override void OnNext()
        {
            FileService file = new FileService();
            Manifest manifest = file.Read<Manifest>(_configs.ProjectManifestPath);
            List<string> requiredModules = manifest.GetScaffoldDependencies();
            _module.requiredModules = requiredModules;
        }

        public override bool ValidateNext()
        {
            return _module.Validate();
        }

    }
}