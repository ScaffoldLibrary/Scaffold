using Newtonsoft.Json.Linq;
using Scaffold.Builder.FileBuilders;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;
using System.IO;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(2)]
    public class AssembliesTab : WindowTab
    {
        public AssembliesTab(BuilderConfigs config) : base(config)
        {
            _assemblies = new List<string>(config.Assemblies);
            _requiredDefines = config.Module.requiredDefines;
        }

        public override string TabKey => "Editing Assemblies";

        private List<string> _assemblies;
        private List<string> _requiredDefines;
        private List<string> _customDefines = new List<string>();

        private bool _hasDependencies;
        public override void OnDraw()
        {
            _hasDependencies = _configs.Module.requiredModules.Any();
        }

        public override void Draw()
        {
            if (!_hasDependencies)
            {
                EditorGUILayout.HelpBox("This module has no dependencies, it doesn't require custom defines", MessageType.Info);
                EditorGUILayout.Space(10);
            }

            EditorGUILayout.LabelField("Select all assembly files used in your module:", ScaffoldStyles.WrappedLabel);
            EditorGUILayout.Space(5);

            int assemblyCount = _assemblies.Count;
            EditorGUILayout.LabelField("Assemblies: ", ScaffoldStyles.CornerLabel);
            for (int i = 0; i < assemblyCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    bool isAssembly = !string.IsNullOrWhiteSpace(_assemblies[i]) && File.Exists(_assemblies[i]);
                    
                    _assemblies[i] = ScaffoldComponents.FileField(_assemblies[i], extension: "*.asmdef", isValidPath: isAssembly);
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

            EditorGUILayout.LabelField("Now, Check if the required defines for this module is correct.\nYou may also add custom defines", ScaffoldStyles.WrappedLabel);
            EditorGUILayout.Space(5);

            ScaffoldComponents.StringList(_requiredDefines, "Defines: ", false);
            EditorGUILayout.Space(-20);
            _customDefines = ScaffoldComponents.StringList(_customDefines);
        }

        public override void OnNext()
        {
            //Save target assemblies
            _assemblies.RemoveAll(a => string.IsNullOrWhiteSpace(a));
            _configs.Assemblies = _assemblies;

            //Save defines to manifest
            List<string> defines = _requiredDefines.Union(_customDefines).Where(d => !string.IsNullOrWhiteSpace(d)).ToList();
            _configs.Module.requiredDefines = defines;
        }

        public override bool ValidateNext()
        {
            if (!_hasDependencies)
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