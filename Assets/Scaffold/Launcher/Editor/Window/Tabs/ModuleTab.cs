using Scaffold.Launcher.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Launcher.Editor
{
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
                    _installedOptions = new List<ModuleOption>() { new ModuleOption("Uninstall", Scaffold.RemoveModule), new ModuleOption("Update", Scaffold.UpdateModule) };
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
                    _uninstalledOptions = new List<ModuleOption>() { new ModuleOption("Install", Scaffold.AddModule), new ModuleOption("Check for Update", Scaffold.CheckForModuleUpdates) };
                }
                return _uninstalledOptions;
            }
        }
        private List<ModuleOption> _uninstalledOptions;

        public override void Draw(Vector2 windowSize, ScaffoldManager scaffold)
        {
            List<Module> modules = scaffold.GetModules();
            bool notConnected = Application.internetReachability == NetworkReachability.NotReachable;

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
                    if (GUILayout.Button("Update Library", ScaffoldStyles.Button, GUILayout.Width(windowSize.x - 170)))
                    {
                        scaffold.UpdateLibrary();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
                DrawModules(modules);
            }
        }

        private void TryDrawDependencyFixer()
        {
            if (Scaffold.CheckForMissingDependencies())
            {
                EditorGUILayout.BeginVertical(ScaffoldStyles.WarningBox);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUIContent icon = EditorGUIUtility.IconContent("_Help");
                        GUILayout.Label(icon, GUILayout.MaxWidth(25));
                        GUILayout.Label("You have missing dependencies, fix now?");
                        if (GUILayout.Button("Fix"))
                        {
                            Scaffold.ResolveDependencies();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawModules(List<Module> modules)
        {
            Dictionary<Module, Version> installedModules = Scaffold.GetInstalledModules();
            EditorGUILayout.BeginVertical(ScaffoldStyles.ProjectStateReady);
            {
                EditorGUILayout.Space(10);
                foreach (Module module in modules)
                {
                    installedModules.TryGetValue(module, out Version version);
                    DrawModuleViewer(module, version);
                    EditorGUILayout.Space(5);
                }
                EditorGUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawProjectState()
        {
            Dictionary<Module, Version> installedModules = Scaffold.GetInstalledModules();
            bool updateNeeded = false;
            foreach (var entry in installedModules)
            {
                if (entry.Value.CompareTo(entry.Key.Latest()) > 0)
                {
                    updateNeeded = true;
                    break;
                }
            }

            string stateLabel;
            GUIStyle style;

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                stateLabel = "No Internet";
                style = ScaffoldStyles.ProjectStateError;
            }
            else if (Scaffold.CheckForMissingDependencies())
            {
                stateLabel = "Missing Dependencies";
                style = ScaffoldStyles.ProjectStateError;
            }
            else if (updateNeeded)
            {
                stateLabel = "Updates available";
                style = ScaffoldStyles.ProjectStatePending;
            }
            else
            {
                stateLabel = "Ready";
                style = ScaffoldStyles.ProjectStateReady;
            }
            EditorGUILayout.LabelField(stateLabel, style, GUILayout.Width(150));
        }

        private void DrawModuleViewer(Module module, Version installedVersion)
        {
            bool installed = installedVersion != null;
            bool outdated = (installed && installedVersion.CompareTo(module.Latest()) < 0);

            EditorGUILayout.BeginVertical(ScaffoldStyles.ModuleBox);
            {
                Rect rect = EditorGUILayout.BeginHorizontal();
                {
                    string iconName = !installed ? "lightRim" : outdated? "orangeLight" : "greenLight";
                    GUIContent icon = EditorGUIUtility.IconContent(iconName);
                    EditorGUILayout.LabelField(icon, GUILayout.Width(20));
                    string version = installed ? installedVersion.ToString() : module.Latest().ToString();
                    if (outdated) version += " (Update Available)";
                    GUILayout.Label($"{module.displayName} - {version}", ScaffoldStyles.ModuleName);
                    DrawModuleOptions(rect, module, installed);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(3);
                GUILayout.Label(module.description, ScaffoldStyles.ModuleDescription);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawModuleOptions(Rect rect, Module module, bool installed)
        {
            List<ModuleOption> options = installed ? InstalledOptions : UninstalledOptions;
            Rect buttonRect = new Rect(rect);
            Vector2 buttonSize = new Vector2(70, 20);
            buttonRect.x = rect.width - 60;
            buttonRect.y -= 1;
            buttonRect.size = buttonSize;

            string[] optionNames = options.Select(o => o.Label).ToArray();
            var popupStyle = GUI.skin.FindStyle("IconButton");
            var popupIcon = EditorGUIUtility.IconContent("_Popup");
            var buttonRect2 = EditorGUILayout.GetControlRect(false, 20f, GUILayout.MaxWidth(20f));
            if (GUI.Button(buttonRect2, popupIcon, popupStyle))
            {
                GenericMenu menu = new GenericMenu();
                foreach(var option in options)
                {
                    menu.AddItem(new GUIContent(option.Label), false, () => { option.Action?.Invoke(module);});
                }
                menu.ShowAsContext();
            }
        }

        public struct ModuleOption
        {
            public ModuleOption(string label, Action<Module> action)
            {
                Label = label;
                Action = action;
            }

            public string Label;
            public Action<Module> Action;
        }
    }
}