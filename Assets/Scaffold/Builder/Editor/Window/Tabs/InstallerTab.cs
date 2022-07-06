using Scaffold.Builder.FileBuilders;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(3)]
    public class InstallerTab : WindowTab
    {
        public InstallerTab(BuilderConfigs config) : base(config)
        {

        }

        private Module _module;

        public override string TabKey => "Creating Installers...";

        //Main Folder
        private string _installerFolder = string.Empty;

        //Files Folders
        private bool _hasInstaller;
        private string _installerPath = string.Empty;

        private bool _hasAssembly;
        private string _assemblyPath = string.Empty;


        //Installation Defines
        private List<string> _installDefines = new List<string>();
        private List<string> _customInstallDefines = new List<string>();


        public override void OnDraw()
        {
            _module = _configs.Module;
            _installDefines = _module.installDefines;
            _installerPath = _configs.InstallerPath;
            _assemblyPath = _configs.InstallerAssemblyPath;

            if (!string.IsNullOrWhiteSpace(_configs.InstallerPath))
            {
                string fileName = Path.GetFileName(_configs.InstallerPath);
                _installerFolder = _configs.InstallerPath.Substring(0, _configs.InstallerPath.LastIndexOf(fileName));
            }
        }

        public override void Draw()
        {
            bool hasFolder = !string.IsNullOrWhiteSpace(_installerFolder);
            _hasInstaller = !string.IsNullOrWhiteSpace(_installerPath) && File.Exists(_installerPath);
            _hasAssembly = !string.IsNullOrWhiteSpace(_assemblyPath) && File.Exists(_assemblyPath);

            _installerFolder = ScaffoldComponents.FolderField(_installerFolder, "Installer Folder: ", isValidPath: hasFolder);
            EditorGUILayout.Space(5);

            if (hasFolder && !Directory.Exists(_installerFolder))
            {
                EditorGUILayout.LabelField("The folder does not exist, do you wish to create it?");
                Rect rect = EditorGUILayout.GetControlRect();
                rect.width = 100;
                if (GUI.Button(rect, "Create Folder"))
                {
                    Directory.CreateDirectory(_installerFolder);
                }
                return;
            }

            EditorGUI.BeginDisabledGroup(!hasFolder);
            {
                EditorGUILayout.Space(10);

                string newInstallerPath = ScaffoldComponents.FileSearchOrCreate(_installerPath, "Installer: ", _hasInstaller, _installerFolder, "cs", CreateInstaller);
                if (!string.IsNullOrWhiteSpace(newInstallerPath))
                {
                    if (newInstallerPath.Contains(_installerFolder))
                    {
                        _installerPath = newInstallerPath;
                    }
                    else
                    {
                        Debug.Log("Installer file must be inside the designated installer folder");
                        _installerPath = string.Empty;
                    }
                }

                string newAssemblyPath = ScaffoldComponents.FileSearchOrCreate(_assemblyPath, "Assembly: ", _hasAssembly, _installerFolder, "asmdef", CreateAssembly);
                if (!string.IsNullOrWhiteSpace(newAssemblyPath))
                {
                    if (newAssemblyPath.Contains(_installerFolder))
                    {
                        _assemblyPath = newAssemblyPath;
                    }
                    else
                    {
                        Debug.Log("Assembly file must be inside the designated installer folder");
                        _assemblyPath = string.Empty;
                    }
                }

                EditorGUILayout.Space(10);

                ScaffoldComponents.StringList(_installDefines, "Installation Defines: ", false);
                EditorGUILayout.Space(-20);
                _customInstallDefines = ScaffoldComponents.StringList(_customInstallDefines);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void CreateInstaller()
        {
            string moduleName = _module.GetPascalName();
            _installerPath = $"{_installerFolder}/{moduleName}Installer.cs";
            if (File.Exists(_installerPath))
            {
                Debug.Log("File already exists");
                return;
            }
            File.WriteAllText(_installerPath, string.Empty);
        }

        private void CreateAssembly()
        {
            string moduleName = _module.GetPascalName();
            string rawAssembly = _configs.TemplateAssembly;
            rawAssembly = rawAssembly.Replace("ModuleName", moduleName);
            _assemblyPath = $"{_installerFolder}/Scaffold.{moduleName}.Installer.asmdef";
            if (File.Exists(_assemblyPath))
            {
                Debug.Log("File already exists");
                return;
            }
            File.WriteAllText(_assemblyPath, rawAssembly);
        }

        public override void OnNext()
        {
            //set paths
            _configs.InstallerAssemblyPath = _assemblyPath;
            _configs.InstallerPath = _installerPath;

            //cleanup assembly from selection
            _configs.Assemblies.Remove(_assemblyPath);
            
            _installDefines.AddRange(_customInstallDefines);
            _module.installDefines = _installDefines;
        }

        public override bool ValidateNext()
        {
            return (_hasAssembly && _hasInstaller);
        }

    }
}