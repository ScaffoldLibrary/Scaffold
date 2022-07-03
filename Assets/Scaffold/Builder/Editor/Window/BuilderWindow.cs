using Scaffold.Builder.Editor.Tabs;
using Scaffold.Builder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Module;

namespace Scaffold.Builder.Editor
{
    public class BuilderWindow : EditorWindow
    {
        public static void OpenBuilder()
        {
            Window.Show();
            Window.minSize = _minWindowSize;
        }

        private static Vector2 CurrentWindowSize => Window.position.size;
        private static Vector2 _minWindowSize = new Vector2(400, 400);

        private static BuilderWindow Window
        {
            get
            {
                if (_window == null)
                {
                    _window = (BuilderWindow)EditorWindow.GetWindow(typeof(BuilderWindow));
                }
                return _window;
            }
        }
        private static BuilderWindow _window;

        private WindowTab CurrentTab => Tabs[_currentTabIndex];
        private int _currentTabIndex;

        private List<WindowTab> Tabs
        {
            get
            {
                if (_tabs == null)
                {
                    _tabs = GetAllTabs();
                }
                return _tabs;
            }
        }
        private List<WindowTab> _tabs;

        //Tab animation values
        private float _currentValue;
        private float _targetValue;
        private bool _animating;

        private Module module;
        private Vector2 _scroll;

        private void OnGUI()
        {
            GUILayout.Box("Scaffold Builder", ScaffoldStyles.HeaderBox);
            DrawProgress(CurrentTab.TabKey);
            EditorGUILayout.Space(10);
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            {
                CurrentTab.Draw();
            }
            DrawFooterButtons();
            EditorGUILayout.EndScrollView();
        }

        private void DrawProgress(string description)
        {
            float step = (1f / Tabs.Count) * (_currentTabIndex + 1);
            _targetValue = step;
            AnimateValue();
            Rect r = EditorGUILayout.BeginVertical();
            r.x += 10;
            r.width -= 20;
            EditorGUI.ProgressBar(r, _currentValue, description);
            GUILayout.Space(18);
            EditorGUILayout.EndVertical();
        }

        private async void AnimateValue()
        {
            if (_animating) return;
            _animating = true;
            while (_currentValue != _targetValue)
            {
                Repaint();
                if (Mathf.Abs((_currentValue - _targetValue)) > 0.01f)
                {
                    float diff = _targetValue - _currentValue;
                    float step = (diff / Mathf.Abs(diff)) * 0.02f;
                    _currentValue += step;
                    if (diff < 0)
                    {
                        _currentValue = Mathf.Clamp(_currentValue, _targetValue, 1);
                    }
                    else
                    {
                        _currentValue = Mathf.Clamp(_currentValue, 0, _targetValue);
                    }
                }
                await Task.Delay(1);
            }
            _animating = false;
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

        private List<WindowTab> GetAllTabs()
        {
            List<WindowTab> tabs = new List<WindowTab>();
            List<Type> types = GetAllTabTypes();
            foreach (Type type in types)
            {
                object[] parameters = new object[] { ScaffoldBuilder.Config };
                WindowTab tab = Activator.CreateInstance(type, parameters) as WindowTab;
                tabs.Add(tab);
            }
            return tabs;
        }

        private List<WindowTab> CreateTabs()
        {
            BuilderConfigs config = ScaffoldBuilder.Config;
            List<WindowTab> tabs = new List<WindowTab>()
            {
                new StartupTab(config),
                new ManifestTab(config),
                new AssembliesTab(config),
                new InstallerTab(config),
                new UploadTab(config)
            };
            return tabs;
        }

        private void DrawFooterButtons()
        {
            GUILayout.FlexibleSpace();
            Rect spacing = EditorGUILayout.BeginHorizontal();
            {
                if (_currentTabIndex > 0)
                {
                    if (GUILayout.Button("Back", GUILayout.MaxWidth(150)))
                    {
                        Back();
                    }
                }

                EditorGUILayout.Space(spacing.width - 300);

                EditorGUI.BeginDisabledGroup(!ValidateNext());
                {
                    string buttonState = _currentTabIndex >= Tabs.Count - 1 ? "Finish" : "Next";
                    if (GUILayout.Button(buttonState, GUILayout.MaxWidth(150)))
                    {
                        Next();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }

        private void Back()
        {
            GUI.FocusControl(null);
            _currentTabIndex -= 1;
            _scroll.y = 0;
            CurrentTab.OnDraw();
        }

        private bool ValidateNext()
        {
            return CurrentTab.ValidateNext();
        }

        private void Next()
        {
            GUI.FocusControl(null);
            CurrentTab.OnNext();
            _currentTabIndex += 1;
            _scroll.y = 0;
            if (_currentTabIndex >= Tabs.Count)
            {
                Window.Close();
                return;
            }
            else
            {
                CurrentTab.OnDraw();
            }
        }
    }
}
