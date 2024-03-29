﻿using Scaffold.Builder.Utilities;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;
using System.Collections;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    public class BuildTab : WindowTab
    {
        public BuildTab(BuilderConfigs config) : base(config)
        {

        }

        public override string TabKey => "Building module...";


        private bool _hasBuilt;


        public override void OnDraw()
        {

        }

        public override void Draw()
        {
            EditorGUILayout.LabelField("Your Module is ready to be deployed!\n\nPress Build to finish building your files and upload your module manifest to the server", ScaffoldStyles.WrappedLabel);

            EditorGUILayout.Space(10);

            if (_hasBuilt)
            {
                EditorGUILayout.LabelField("Module Ready! Press Finish");
                return;
            }

            if (GUILayout.Button("Build"))
            {
                _hasBuilt = true;
                FileService file = new FileService();
                file.Save(_configs.Module, _configs.ModuleManifestPath);

                Credentials credentials = file.Read<Credentials>(_configs.CredentialsPath);
                ModuleUploader.UploadModule(_configs.Module, credentials);

                ScaffoldBuilder.QuickBuild();
            }
        }


        public override void OnNext()
        {

        }

        public override bool ValidateNext()
        {
            return _hasBuilt;
        }
    }
}