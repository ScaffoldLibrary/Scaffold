using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Builder.Editor;
using Scaffold.Builder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor.Module;

namespace Scaffold.Builder
{
    public class ScaffoldBuilder
    {
        public static BuilderConfigs Config
        {
            get
            {
                if (_config == null)
                {
#if !IS_SCAFFOLD_BUILDER
                    _config = AssetDatabase.LoadAssetAtPath<BuilderConfigs>("Package/com.scaffold.builder/Editor/Config/BuilderConfigs.asset");
#else
                    _config = AssetDatabase.LoadAssetAtPath<BuilderConfigs>("Assets/Scaffold/Builder/Editor/Config/BuilderConfigs.asset");
#endif
                }
                return _config;
            }
        }
        private static BuilderConfigs _config;

        [MenuItem("Scaffold/Builder/Build", priority = 0)]
        private static void Build()
        {
            BuilderWindow.OpenBuilder();
        }

        [MenuItem("Scaffold/Builder/Quick Build %#Q", priority = 1)]
        private static void QuickBuild()
        {
            if (!ValidateModuleStructure())
            {
                return;
            }
            ChangeVersionNumber();
            BuildModuleFiles();
            UploadModuleManifest();
        }

        [MenuItem("Scaffold/Builder/Quick Build %#Q", true)]
        private static bool ValidateQuickBuild()
        {
            return false;
        }

        [MenuItem("Scaffold/Builder/Build Steps/Validate Module Structure")]
        private static bool ValidateModuleStructure()
        {
            //check for package
            if (!CheckPackageManifest())
            {
                return false;
            }

            //check for assemblies
            if (Directory.GetFiles("./Assets/", "*.asmdef", SearchOption.AllDirectories).Length <= 0)
            {
                Debug.LogError("Make sure you have at least one assembly in your project");
            }

            return true;
        }

        private static bool CheckPackageManifest()
        {
            if (_config.Module == null)
            {
                Debug.LogError("Make sure you have a valid Package Manifest file created");
                return false;
            }

            if (_config.Module.Validate())
            {
                Debug.LogError("Empty fields on the project manifest, please check");
                return false;
            }

            return true;
        }

        private static bool ChangeVersionNumber()
        {
            var version = EditorInputDialog.Show("Building Module", "Please enter the new version", "");
            if (!string.IsNullOrEmpty(version))
            {
                return false;
            }
            if (Version.TryParse(version, out Version v))
            {
                _config.Module.version = version;
                return true;
            }

            return false;
        }

        [MenuItem("Scaffold/Builder/Build Steps/Build Files")]
        private static void BuildModuleFiles()
        {
            //ModuleBuilder.Build(Config);
        }

        [MenuItem("Scaffold/Builder/Build Steps/Upload Manifest")]
        private static void UploadModuleManifest()
        {
            if (!CheckPackageManifest())
            {
                Debug.Log("Failed to read module manifest");
                return;
            }

            ModuleUploader.UploadModule(Config);
        }

        [MenuItem("Scaffold/Builder/Build Steps/Create Manifest File")]
        public static void CreateManifest()
        {
            string path = Config.ManifestPath;
            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.OpenFolderPanel("Select folder", "", "");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
            }

            //Config.Manifest.Save(path);
            Debug.Log($"Created manifest file at {path}");
        }

        [MenuItem("Scaffold/Builder/Config/Reset Manifest")]
        private static void ResetManifest()
        {
            //Config.ResetManifest();
        }
    }
}
