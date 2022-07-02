using Scaffold.Builder.Editor.Components;
using Scaffold.Builder.FileBuilders;
using Scaffold.Builder.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(3)]
    public class InstallerTab : WindowTab
    {
        public InstallerTab(BuilderWindow window, BuilderConfigs config) : base(window, config)
        {
            _Installerbuilder = new InstallerBuilder(config);
            _assemblyBuilder = new AssemblyBuilder(config);
            _installDefines = config.InstallDefines;

            if (string.IsNullOrWhiteSpace(config.InstallerPath))
            {
                _installerFolder = Path.GetDirectoryName(config.InstallerPath);
            }
        }

        public override string TabKey => "Creating Installers...";

        //Main Folder
        private string _installerFolder;

        //Files Folders
        private bool _hasInstaller;
        private string _installerPath;
        private string _installerName;

        private bool _hasAssembly;
        private string _assemblyPath;
        private string _assemblyName;


        //Installation Defines
        private List<string> _installDefines = new List<string>();
        private List<string> _customInstallDefines = new List<string>();

        //Builders
        private InstallerBuilder _Installerbuilder;
        private AssemblyBuilder _assemblyBuilder;

        public override void Draw()
        {
            _installerFolder = EditorComponents.FolderField(_installerFolder, "Installer Folder: ");
            EditorGUILayout.Space(5);
            if (string.IsNullOrEmpty(_installerFolder))
            {
                return;
            }

            if (!Directory.Exists(_installerFolder))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("The folder don't exists, do you wish to create it?");
                    if (GUILayout.Button("Create Folder"))
                    {
                        Directory.CreateDirectory(_installerFolder);
                    }
                }
                EditorGUILayout.EndHorizontal();
                return;
            }

            if (string.IsNullOrWhiteSpace(_installerPath))
            {
                ResetInstallerPath();
            }


            _hasInstaller = GetFile(_installerName);
            if (_hasInstaller)
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        var icon = EditorGUIUtility.IconContent("greenLight");
                        icon.text = "  Installer: ";
                        EditorGUILayout.TextField(icon, _installerName);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        var icon = EditorGUIUtility.IconContent("redLight");
                        icon.text = "  Installer: ";
                        EditorGUILayout.LabelField(icon);
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Search"))
                    {
                        string path = EditorUtility.OpenFilePanel("Select Installer File", _installerFolder, "cs");
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            if (Directory.GetParent(path).FullName == _installerFolder)
                            {
                                _installerPath = path;
                                _installerName = Path.GetFileName(path);
                            }
                            else
                            {
                                Debug.Log("The selected file must be inside the installer folder");
                            }
                        }
                    }
                    if (GUILayout.Button("Create"))
                    {
                        ResetInstallerPath();
                        string rawInstaller = _Installerbuilder.GetRawInstaller();
                        _Installerbuilder.WriteInstaller(rawInstaller);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (string.IsNullOrWhiteSpace(_assemblyPath))
            {
                ResetAssemblyPath();
            }

            _hasAssembly = GetFile(_assemblyName);
            if (_hasAssembly)
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        var icon = EditorGUIUtility.IconContent("greenLight");
                        icon.text = "  Assembly: ";
                        EditorGUILayout.TextField(icon, _assemblyName);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        var icon = EditorGUIUtility.IconContent("redLight");
                        icon.text = "  Assembly: ";
                        EditorGUILayout.LabelField(icon);
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Search"))
                    {
                        string path = EditorUtility.OpenFilePanel("Select Assembly File", _installerFolder, "asmdef");
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            if (Path.GetDirectoryName(path) == _installerFolder)
                            {
                                _assemblyPath = path;
                                _assemblyName = Path.GetFileName(path);
                            }
                            else
                            {
                                Debug.Log("The selected file must be inside the installer folder");
                            }
                        }
                    }
                    if (GUILayout.Button("Create"))
                    {
                        _configs.InstallerAssemblyPath = _assemblyPath;
                        _assemblyBuilder.Build();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(10);

            EditorComponents.StringList(_installDefines, "Installation Defines: ", false);
            EditorGUILayout.Space(-20);
            _customInstallDefines = EditorComponents.StringList(_customInstallDefines);
        }

        private bool GetFile(string fileName)
        {
            var files = Directory.GetFiles(_installerFolder, fileName, SearchOption.TopDirectoryOnly);
            return files.Length > 0;
        }

        private void ResetInstallerPath()
        {
            _installerName = "";
            _installerPath = "";
        }

        private void ResetAssemblyPath()
        {
            _assemblyName = "";
            _assemblyPath = "";
        }

        public override void OnNext()
        {
            _configs.InstallerPath = _installerFolder;
            _installDefines.AddRange(_customInstallDefines);
            _configs.InstallDefines = _installDefines;
            _configs.Manifest.Save(_configs.ManifestPath);

            //Update Installer
            _Installerbuilder.Build();

            //UpdateAssembly
            _assemblyBuilder.Build();
        }

        public override bool ValidateNext()
        {
            return (_hasAssembly && _hasInstaller);
        }
    }
}