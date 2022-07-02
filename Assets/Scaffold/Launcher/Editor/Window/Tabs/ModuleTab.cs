using Scaffold.Launcher.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;

namespace Scaffold.Launcher.Editor
{
    [TabOrder(0)]
    public class ModuleTab : WindowTab
    {
        public ModuleTab(Vector2 windowSize, ScaffoldManager scaffold) : base(windowSize, scaffold) { }
        public override string TabName => "Modules";

        private List<ModuleOption> InstalledOptions
        {
            get
            {
                if (_installedOptions == null)
                {
                    _installedOptions = new List<ModuleOption>() { new ModuleOption("Options", null), new ModuleOption("Uninstall", Scaffold.UninstallModule), new ModuleOption("Update", Scaffold.UpdatetInstalledModule) };
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
                    _uninstalledOptions = new List<ModuleOption>() { new ModuleOption("Options", null), new ModuleOption("Install", Scaffold.InstallModule), new ModuleOption("Check for Update", Scaffold.CheckForModuleUpdate) };
                }
                return _uninstalledOptions;
            }
        }
        private List<ModuleOption> _uninstalledOptions;

        public override void Draw(Vector2 windowSize, ScaffoldManager scaffold)
        {
            List<ScaffoldModule> modules = scaffold.GetModules();
            bool notConnected = Application.internetReachability == NetworkReachability.NotReachable;
            if (notConnected)
            {
                EditorGUILayout.HelpBox("Please check your internet connection, connection is needed to install and update the packages", MessageType.Error);
                EditorGUILayout.Space();
            }

            EditorGUI.BeginDisabledGroup(notConnected || scaffold.Busy);
            {
                TryDrawDependencyFixer();
                EditorGUILayout.BeginHorizontal();
                {
                    DrawProjectState();
                    if (GUILayout.Button("Update Manifest", ScaffoldStyles.Button, GUILayout.Width(windowSize.x - 170)))
                    {
                        scaffold.UpdateManifest();
                    }
                }
                EditorGUILayout.EndHorizontal();
                DrawModules(modules);
            }
        }

        private void TryDrawDependencyFixer()
        {
            if (Scaffold.CheckForMissingDependencies())
            {
                Rect module = EditorGUILayout.BeginVertical(ScaffoldStyles.WarningBox);
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
            EditorGUILayout.BeginVertical(ScaffoldStyles.ProjectStateReady);
            foreach (ScaffoldModule package in modules)
            {
                bool installed = Scaffold.IsModuleInstalled(package);
                DrawModuleViewer(package, installed);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawProjectState()
        {
            string stateLabel = "";
            GUIStyle style;
            if (Scaffold.Busy)
            {
                stateLabel = "Loading...";
                style = ScaffoldStyles.ProjectStateReady;
            }
            else if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                stateLabel = "No Internet";
                style = ScaffoldStyles.ProjectStateError;
            }
            else if (Scaffold.CheckForMissingDependencies())
            {
                stateLabel = "Missing Dependencies";
                style = ScaffoldStyles.ProjectStateError;
            }
            else if (!Scaffold.IsProjectUpToDate())
            {
                stateLabel = "Updates available";
                style = ScaffoldStyles.ProjectStatePending;
            }
            else if (1 + 2 == 3)
            {
                stateLabel = "Ready";
                style = ScaffoldStyles.ProjectStateReady;
            }
            EditorGUILayout.LabelField(stateLabel, style, GUILayout.Width(150));
        }

        private void DrawModuleViewer(ScaffoldModule module, bool installed)
        {
            float maxWidth = WindowSize.x;
            Rect rect = EditorGUILayout.BeginHorizontal(ScaffoldStyles.ModuleBox);
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3 * 2));
                {
                    string version = installed ? module.InstalledVersion : module.LatestVersion;
                    if (module.IsOutdated()) version += " (Update Available)";
                    GUILayout.Label($"{module.Name} - {version}", ScaffoldStyles.ModuleName);
                    GUILayout.Label(module.Description, ScaffoldStyles.ModuleDescription);
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
}