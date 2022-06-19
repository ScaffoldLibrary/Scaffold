using UnityEditor;
using UnityEngine;
using Scaffold.Launcher.PackageHandler;
using Scaffold.Launcher;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Scaffold.Launcher.Editor
{
    internal class ScaffoldWindow : EditorWindow
    {
        [MenuItem("Scaffold/Open Launcher")]
        public static void OpenLauncher()
        {
            Window.Show();
            Window.minSize = _minWindowSize;
        }

        private static Vector2 CurrentWindowSize => Window.position.size;
        private static Vector2 _minWindowSize = new Vector2(400, 400);

        private static ScaffoldWindow Window
        {
            get
            {
                if (_window == null)
                {
                    _window = (ScaffoldWindow)EditorWindow.GetWindow(typeof(ScaffoldWindow));
                }
                return _window;
            }
        }
        private static ScaffoldWindow _window;
        private static ScaffoldManager Scaffold
        {
            get
            {
                if (_scaffold == null) _scaffold = new ScaffoldManager();
                return _scaffold;
            }
        }
        private static ScaffoldManager _scaffold;

        private Vector2 _scrollView;

        private List<ModuleOption> InstalledOptions
        {
            get
            {
                if (_installedOptions == null)
                {
                    _installedOptions = new List<ModuleOption>() { new ModuleOption("Options", null), new ModuleOption("Uninstall", Scaffold.UninstallModule), new ModuleOption("Update", Scaffold.UpdateModule) };
                }
                return _installedOptions;
            }
        }
        private List<ModuleOption> _installedOptions;

        private List<ModuleOption> UninstalledOptions
        {
            get
            {
                if (_uninstalledOptions == null)
                {
                    _uninstalledOptions = new List<ModuleOption>() { new ModuleOption("Options", null), new ModuleOption("Install", Scaffold.InstallModule), new ModuleOption("Check for Update", Scaffold.CheckForUpdates) };
                }
                return _uninstalledOptions;
            }
        }
        private List<ModuleOption> _uninstalledOptions;

        private void OnGUI()
        {
            List<ScaffoldModule> modules = Scaffold.GetModules();
            ScaffoldModule launcher = Scaffold.GetLauncher();
            bool notConnected = Application.internetReachability == NetworkReachability.NotReachable;

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUIStyle.none, GUIStyle.none);
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Box("Scaffold", EditorStyles.HeaderBox);
                    EditorGUILayout.LabelField(launcher.InstalledVersion, EditorStyles.CenterLabel);
                    if (notConnected)
                    {
                        EditorGUILayout.HelpBox("Please check your internet connection, connection is needed to install and update the packages", MessageType.Error);
                        EditorGUILayout.Space();
                    }

                    EditorGUI.BeginDisabledGroup(notConnected);
                    {
                        TryDrawDependencyFixer();
                        EditorGUILayout.BeginHorizontal();
                        {
                            DrawProjectState();
                            if (GUILayout.Button("Update Modules", EditorStyles.Button, GUILayout.Width(CurrentWindowSize.x - 170)))
                            {
                                Scaffold.CheckForUpdates();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        DrawModules(modules);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        private void TryDrawDependencyFixer()
        {
            if (Scaffold.CheckForMissingDependencies())
            {
                Rect module = EditorGUILayout.BeginVertical(EditorStyles.WarningBox);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUIContent icon = EditorGUIUtility.IconContent("_Help");
                        GUILayout.Label(icon, GUILayout.MaxWidth(25));
                        GUILayout.Label("You have missing dependencies, fix now?");
                        if (GUILayout.Button("Fix"))
                        {
                            Scaffold.InstallMissingDependencies();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawModules(List<ScaffoldModule> modules)
        {
            foreach (ScaffoldModule package in modules)
            {
                bool installed = Scaffold.IsModuleInstalled(package);
                DrawModuleViewer(package, installed);
            }

        }

        private void DrawProjectState()
        {
            EditorGUILayout.LabelField("Package State", EditorStyles.ProjectState, GUILayout.Width(150));
        }

        private void DrawModuleViewer(ScaffoldModule module, bool installed)
        {
            float maxWidth = CurrentWindowSize.x;
            Rect rect = EditorGUILayout.BeginHorizontal(EditorStyles.ModuleBox);
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3 * 2));
                {
                    string version = installed ? module.InstalledVersion : module.LatestVersion;
                    GUILayout.Label($"{module.Name} - {version}", EditorStyles.ModuleName);
                    GUILayout.Label(module.Description, EditorStyles.ModuleDescription);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3));
                {
                    DrawModuleOptions(rect, module, installed);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawModuleOptions(Rect rect, ScaffoldModule module, bool installed)
        {
            List<ModuleOption> options = installed ? InstalledOptions : UninstalledOptions;
            Rect buttonRect = new Rect(rect);
            Vector2 buttonSize = new Vector2(100, 20);
            buttonRect.x = buttonRect.width - buttonSize.x;
            buttonRect.y += 5;
            buttonRect.size = buttonSize;

            string[] optionNames = options.Select(o => o.Label).ToArray();
            int selectedIndex = EditorGUI.Popup(buttonRect, 0, optionNames);
            if (selectedIndex > 0)
            {
                options[selectedIndex].Action?.Invoke(module);
            }
        }
    }

    public struct ModuleOption
    {
        public ModuleOption(string label, Action<ScaffoldModule> action)
        {
            Label = label;
            Action = action;
        }

        public string Label;
        public Action<ScaffoldModule> Action;
    }
}