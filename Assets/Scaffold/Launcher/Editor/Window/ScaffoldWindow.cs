using UnityEditor;
using UnityEngine;
using Scaffold.Launcher.Workers;
using Scaffold.Launcher;
using System.Collections.Generic;
using System;
using System.Linq;
using Scaffold.Launcher.Objects;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Launcher.Editor
{
    internal class ScaffoldWindow : EditorWindow
    {
        public static void OpenLauncher(ScaffoldManager scaffold)
        {
            Window = (ScaffoldWindow)EditorWindow.GetWindow(typeof(ScaffoldWindow));
            Window.minSize = _minWindowSize;
            _Scaffold = scaffold;
            Window.Show();
        }

        private static Vector2 CurrentWindowSize => Window.position.size;
        private static Vector2 _minWindowSize = new Vector2(400, 400);

        private static ScaffoldWindow Window;

        private static ScaffoldManager _Scaffold;

        private Vector2 _scrollView;

        private WindowTab CurrentTab => _tabs[_currentTabIndex];
        private int _currentTabIndex;

        private List<WindowTab> _tabs;

        private bool _initialized;

        private void Initialize()
        {
            _initialized = true;
            _tabs = GetAllTabs();
        }

        private void OnGUI()
        {
            try
            {
                if (!_initialized)
                {
                    Initialize();
                }

                Module launcher = _Scaffold.GetLauncher();

                _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUIStyle.none, GUIStyle.none);
                {
                    EditorGUILayout.BeginVertical();
                    {
                        GUILayout.Box("Scaffold", ScaffoldStyles.HeaderBox);
                        EditorGUILayout.LabelField(launcher.version, ScaffoldStyles.CenterLabel);
                        EditorGUILayout.BeginHorizontal();
                        {
                            foreach (WindowTab tab in _tabs)
                            {
                                string tabName = tab.TabName;
                                bool selected = CurrentTab == tab;
                                EditorGUI.BeginDisabledGroup(selected);
                                {
                                    if (GUILayout.Button(tabName))
                                    {
                                        _currentTabIndex = _tabs.IndexOf(tab);
                                    }
                                }
                                EditorGUI.EndDisabledGroup();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        CurrentTab.Draw(CurrentWindowSize, _Scaffold);
                        EditorGUILayout.Space(5);
                        DrawFooter();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            catch
            {
                Close();
            }
        }

        private void DrawFooter()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Website"))
                {
                    //TODO: Website Link
                }
                if (GUILayout.Button("GitHub"))
                {
                    //TODO: GIT Link
                }
                if (GUILayout.Button("License"))
                {
                    //TODO: License Link
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private List<WindowTab> GetAllTabs()
        {
            List<WindowTab> tabs = new List<WindowTab>();
            List<Type> types = GetAllTabTypes();
            foreach (Type type in types)
            {
                object[] parameters = new object[] { CurrentWindowSize, _Scaffold };
                WindowTab tab = Activator.CreateInstance(type, parameters) as WindowTab;
                tabs.Add(tab);
            }
            return tabs;
        }
        private List<Type> GetAllTabTypes()
        {
            Type type = typeof(WindowTab);
            return GetType().Assembly.GetTypes()
                                     .Where(t => t.IsSubclassOf(type))
                                     .Where(t => !t.IsAbstract)
                                     .OrderBy(t => (t.GetCustomAttributes(typeof(TabOrder), true).FirstOrDefault() as TabOrder).Order)
                                     .ToList();
        }
    }
}