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
using Scaffold.Core.Editor.Module;
using Scaffold.Core.Editor;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(1)]
    public class ManifestTab : WindowTab
    {
        public ManifestTab(BuilderConfigs config) : base( config)
        {
            _module = config.Module;
        }

        private Module _module;

        public override string TabKey => "Creating Manifest...";

        public override void Draw()
        {
            _module.name = EditorGUILayout.TextField("Name: ", _module.name);
            _module.displayName = EditorGUILayout.TextField("Display Name: ", _module.displayName);
            _module.path = EditorGUILayout.TextField("Path: ", _module.path);
            _module.unity = EditorGUILayout.TextField("Unity Version: ", _module.unity);
            _module.version = EditorGUILayout.TextField("Version: ", _module.version);
            _module.description = EditorGUILayout.TextArea("Description: ", _module.description);
        }

        public override void OnNext()
        {
            ModuleReader reader = new ModuleReader("");
            Debug.LogError("FAKE READER EXPOSED");
            List<string> dependencies = new List<string>(); 
            _module.requiredModules = dependencies;
            List<string> requiredDefines = ConvertDependenciesToDefines(dependencies);
            _module.requiredDefines = requiredDefines;
            List<string> installDefines = new List<string>() { /*_configs.ModuleDefine */ "" };
            _module.installDefines = installDefines;
            //_manifest.Save(_configs.ManifestPath);
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
            return _module.Validate();
        }
    }
}