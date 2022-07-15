using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Editor
{
    public class ConfigurationTab : WindowTab
    {
        public ConfigurationTab(Vector2 windowSize, ScaffoldManager scaffold) : base(windowSize, scaffold) { }

        public override string TabName => "Configs";

        public override void Draw(Vector2 windowSize, ScaffoldManager scaffold)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Install something: ", GUILayout.Width(150));
                if (GUILayout.Button("install"))
                {

                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Install something: ", GUILayout.Width(150));
                if (GUILayout.Button("install"))
                {

                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}