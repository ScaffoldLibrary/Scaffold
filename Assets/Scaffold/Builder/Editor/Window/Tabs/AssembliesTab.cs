using Newtonsoft.Json.Linq;
using Scaffold.Builder.FileBuilders;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Module;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(2)]
    public class AssembliesTab : WindowTab
    {
        public AssembliesTab(BuilderConfigs config) : base(config)
        {
            _builder = new DefinesBuilder(config);
            _assemblies = new List<string>(config.Assemblies);
            if (_assemblies.Count <= 0) _assemblies.Add("");
            _dependencies = config.Module.requiredModules;
            _requiredDefines = config.Module.requiredDefines;
            _module = config.Module;
        }

        private Module _module;

        public override string TabKey => "Editing Assemblies";

        private DefinesBuilder _builder;

        private List<string> _assemblies;
        private List<string> _dependencies;
        private List<string> _requiredDefines;
        private List<string> _customDefines = new List<string>();

        public override void Draw()
        {
            if (!_dependencies.Any(s => !string.IsNullOrEmpty(s)))
            {
                EditorGUILayout.HelpBox("This module has no dependencies, it doesn't require custom defines", MessageType.Info);
                EditorGUILayout.Space(10);
            }

            int assemblyCount = _assemblies.Count;
            EditorGUILayout.LabelField("Assemblies: ", ScaffoldStyles.CornerLabel);
            for (int i = 0; i < assemblyCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _assemblies[i] = ScaffoldComponents.FileField(_assemblies[i], extension: "*.asmdef");
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        _assemblies.Remove(_assemblies[i]);
                        i -= 1;
                        assemblyCount -= 1;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Assembly"))
            {
                _assemblies.Add("");
            }

            EditorGUILayout.Space(10);

            ScaffoldComponents.StringList(_requiredDefines, "Defines: ", false);
            EditorGUILayout.Space(-20);
            _customDefines = ScaffoldComponents.StringList(_customDefines);
        }

        public override void OnNext()
        {
            _assemblies.RemoveAll(a => string.IsNullOrWhiteSpace(a));
            _configs.Assemblies = _assemblies;

            List<string> defines = new List<string>();
            defines.AddRange(_requiredDefines);
            defines.AddRange(_customDefines);
            defines.RemoveAll(d => string.IsNullOrWhiteSpace(d));
            _module.requiredDefines = defines;

            //_configs.Manifest.Save(_configs.ManifestPath);

            _builder.AddDefinesToAssemblies(_assemblies, defines);
        }

        public override bool ValidateNext()
        {
            if (_dependencies == null  || _dependencies.Count <= 0)
            {
                return true;
            }

            if (_assemblies.Count(a => !string.IsNullOrWhiteSpace(a)) > 0)
            {
                return true;
            }

            return true;
        }
    }
}