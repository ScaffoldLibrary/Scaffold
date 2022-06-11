using UnityEditor;
using UnityEngine;
using Scaffold.Launcher.PackageHandler;
using Scaffold.Launcher;
using System.Collections.Generic;

namespace Scaffold.Launcher.Editor
{
    public class LauncherWindow : EditorWindow
    {
        [MenuItem("Scaffold/Launcher")]
        private static void OpenWindow()
        {
            Window.Show();
            Window.minSize = _minWindowSize;
        }

        private static Vector2 CurrentWindowSize => Window.position.size;
        private static Vector2 _minWindowSize = new Vector2(400, 400);

        private static LauncherWindow Window
        {
            get
            {
                if (_window == null)
                {
                    _window = (LauncherWindow)EditorWindow.GetWindow(typeof(LauncherWindow));
                }
                return _window;
            }
        }
        private static LauncherWindow _window;
        private static ScaffoldLauncher Launcher
        {
            get
            {
                if (_launcher == null) _launcher = new ScaffoldLauncher();
                return _launcher;
            }
        }
        private static ScaffoldLauncher _launcher;

        private Vector2 _scrollView;

        private void OnGUI()
        {
            List<PackagePath> scaffoldPackages = Launcher.GetPackages();

            _scrollView = EditorGUILayout.BeginScrollView(_scrollView, GUIStyle.none, GUIStyle.none);
            EditorGUILayout.BeginVertical();
            GUILayout.Box("Scaffold", LauncherStyles.HeaderBox);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                EditorGUILayout.HelpBox("Please check your internet connection, connection is needed to install and update the packages", MessageType.Error);
            }

            EditorGUILayout.LabelField("this is my scaffold project description, version 0.1");
            EditorGUILayout.BeginHorizontal();
            DrawProjectState();
            if (GUILayout.Button("Update Modules", LauncherStyles.Button, GUILayout.Width(CurrentWindowSize.x - 170)))
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
            EditorGUILayout.LabelField("Package State", LauncherStyles.ProjectState, GUILayout.Width(150));
        }

        private void DrawModuleViewer(PackagePath package, bool installed)
        {
            float maxWidth = CurrentWindowSize.x;
            Rect module = EditorGUILayout.BeginHorizontal(LauncherStyles.ModuleBox);

            //Div 1
            Rect verticalLeft = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth / 3 * 2));
            GUILayout.Label($"{package.Name} - {package.Version}", LauncherStyles.ModuleName);
            GUILayout.Label(package.Description, LauncherStyles.ModuleDescription);
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
            return GUI.Button(buttonRect, text, LauncherStyles.CornerButton);
        }
    }
}