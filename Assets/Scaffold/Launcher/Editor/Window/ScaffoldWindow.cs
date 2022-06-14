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
        private static ScaffoldManager Launcher
        {
            get
            {
                if (_launcher == null) _launcher = new ScaffoldManager();
                return _launcher;
            }
        }
        private static ScaffoldManager _launcher;

        private Vector2 _scrollView;

        private void OnGUI()
        {
            List<PackagePath> scaffoldPackages = Launcher.GetPackages();

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUIStyle.none, GUIStyle.none);
            EditorGUILayout.BeginVertical();
            GUILayout.Box("Scaffold", EditorStyles.HeaderBox);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                EditorGUILayout.HelpBox("Please check your internet connection, connection is needed to install and update the packages", MessageType.Error);
            }

            EditorGUILayout.LabelField("this is my scaffold project description, version 0.1");
            EditorGUILayout.BeginHorizontal();
            DrawProjectState();
            if (GUILayout.Button("Update Modules", EditorStyles.Button, GUILayout.Width(CurrentWindowSize.x - 170)))
            {
                Launcher.UpdateModules();
            }
            EditorGUILayout.EndHorizontal();
            foreach (PackagePath package in scaffoldPackages)
            {
                bool installed = Launcher.IsPackageInstalled(package);
                DrawModuleViewer(package, installed);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void DrawProjectState()
        {
            EditorGUILayout.LabelField("Package State", EditorStyles.ProjectState, GUILayout.Width(150));
        }

        private void DrawModuleViewer(PackagePath package, bool installed)
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
                Launcher.InstallPackage(package);
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