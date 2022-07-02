using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;

namespace Scaffold.Core.Editor
{
    public static class ScaffoldComponents
    {
        public static List<string> StringList(List<string> list, string label = null, bool editable = true)
        {
            EditorGUILayout.LabelField(label, ScaffoldStyles.CornerLabel);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginDisabledGroup(!editable);
                        {
                            list[i] = EditorGUILayout.TextField(list[i]);
                        }
                        EditorGUI.EndDisabledGroup();
                        if (editable)
                        {
                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                list.Remove(list[i]);
                                i -= 1;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (editable)
            {
                if (GUILayout.Button("Add Entry"))
                {
                    if (list == null) list = new List<string>();
                    list.Add("");
                }
            }
            return list;
        }

        public static string FileField(string folderPath, string label = null, string directory = null, string extension = null)
        {
            directory = directory ?? "";
            extension = extension ?? "";
            string fileName = "";
            if (!string.IsNullOrEmpty(folderPath))
            {
                fileName = Path.GetFileName(folderPath);
            }

            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, ScaffoldStyles.CornerLabel);
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    string iconName = !string.IsNullOrWhiteSpace(folderPath) ? "greenLight" : "redLight";
                    GUIContent icon = EditorGUIUtility.IconContent(iconName);
                    EditorGUILayout.TextField(icon, fileName);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Select File", GUILayout.Width(100)))
                {
                    string path = EditorUtility.OpenFilePanel("Select File", directory, extension);
                    if (!string.IsNullOrEmpty(path))
                    {
                        folderPath = path;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            return folderPath;
        }

        public static string FolderField(string folderPath, string label = null)
        {
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, ScaffoldStyles.CornerLabel);
            }

            EditorGUILayout.BeginHorizontal();
            {
                string iconName = !string.IsNullOrWhiteSpace(folderPath) ? "greenLight" : "redLight";
                GUIContent icon = EditorGUIUtility.IconContent(iconName);
                folderPath = EditorGUILayout.TextField(icon, folderPath);
                if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        folderPath = path;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            return folderPath;
        }
    }
}