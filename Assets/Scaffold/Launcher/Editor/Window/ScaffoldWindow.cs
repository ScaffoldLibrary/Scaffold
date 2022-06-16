using UnityEditor;
using UnityEngine;
using Scaffold.Launcher.PackageHandler;
using Scaffold.Launcher;
using System.Collections.Generic;

namespace Scaffold.Launcher.Editor
{
    public class ScaffoldWindow : EditorWindow
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

        private void OnGUI()
        {
            List<ScaffoldModule> modules = Scaffold.GetModules();
            bool notConnected = Application.internetReachability != NetworkReachability.NotReachable;

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUIStyle.none, GUIStyle.none);
            EditorGUILayout.BeginVertical();
            GUILayout.Box("Scaffold", EditorStyles.HeaderBox);
            EditorGUILayout.LabelField("this is my scaffold project description, version 0.1");
            if (notConnected)
            {
                EditorGUILayout.HelpBox("Please check your internet connection, connection is needed to install and update the packages", MessageType.Error);
                EditorGUILayout.Space();
            }

            EditorGUI.BeginDisabledGroup(notConnected);
            {
                TryDrawDependencyFixer();
                EditorGUILayout.BeginHorizontal();
                DrawProjectState();
                if (GUILayout.Button("Update Modules", EditorStyles.Button, GUILayout.Width(CurrentWindowSize.x - 170)))
                {
                    Scaffold.UpdateModules();
                }
                EditorGUILayout.EndHorizontal();
                DrawModules(modules);
            }
        }

        private void TryDrawDependencyFixer()
        {
            if (Scaffold.CheckForMissingDependencies())
            {
                //draw red box with button to install everything
                Rect module = EditorGUILayout.BeginVertical(EditorStyles.ModuleBox);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("PING");
                GUILayout.Label("You have a few missing dependencies, want to fix it?");
                if (GUILayout.Button("Fix dependencies"))
                {

                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawModules(List<ScaffoldModule> modules)
        {
            foreach (ScaffoldModule package in modules)
            {
                bool installed = Scaffold.IsPackageInstalled(package);
                DrawModuleViewer(package, installed);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawProjectState()
        {
            EditorGUILayout.LabelField("Package State", EditorStyles.ProjectState, GUILayout.Width(150));
        }

        private void DrawModuleViewer(ScaffoldModule package, bool installed)
        {
            float maxWidth = CurrentWindowSize.x;
            Rect module = EditorGUILayout.BeginHorizontal(EditorStyles.ModuleBox);

            //Div 1
            Rect verticalLeft = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3 * 2));
            GUILayout.Label($"{package.Name} - {package.Version}", EditorStyles.ModuleName);
            GUILayout.Label(package.Description, EditorStyles.ModuleDescription);
            EditorGUILayout.EndVertical();

            //Div 2
            Rect verticalRight = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3));
            EditorGUI.BeginDisabledGroup(installed);
            string packageState = installed ? "Installed" : "Install";
            if (CornerButton(packageState, module, verticalRight))
            {
                Debug.Log("Trying to install");
                Scaffold.InstallPackage(package);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private bool CornerButton(string text, Rect divRect, Rect groupRect)
        {
            Rect buttonRect = new Rect(divRect);
            Vector2 buttonSize = new Vector2(100, 20);
            buttonRect.x = buttonRect.width - buttonSize.x;
            buttonRect.y += 5;

            buttonRect.size = buttonSize;
            return GUI.Button(buttonRect, text, EditorStyles.CornerButton);
        }
    }
}