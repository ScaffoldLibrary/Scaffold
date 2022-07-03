using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor;
using System;

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

        public static string FileField(string folderPath, string label = null, string directory = null, string extension = null, bool isValidPath = true)
        {
            directory ??= string.Empty;
            extension ??= string.Empty;
            string fileName = string.Empty;
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
                    string iconName = isValidPath ? "greenLight" : "redLight";
                    GUIContent icon = EditorGUIUtility.IconContent(iconName);
                    EditorGUILayout.LabelField(icon, GUILayout.Width(20));
                    fileName = EditorGUILayout.TextField(fileName);
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

        public static string FolderField(string folderPath, string label = null, bool isValidPath = true)
        {
            if (!string.IsNullOrWhiteSpace(label))
            {
                EditorGUILayout.LabelField(label, ScaffoldStyles.CornerLabel);
            }

            EditorGUILayout.BeginHorizontal();
            {
                string iconName = isValidPath ? "greenLight" : "redLight";
                GUIContent icon = EditorGUIUtility.IconContent(iconName);
                EditorGUILayout.LabelField(icon, GUILayout.Width(20));
                folderPath = EditorGUILayout.TextField(folderPath);
                if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        folderPath = path;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            return folderPath;
        }

        public static string FileSearchOrCreate(string filePath, string label = null, bool isValid = true, string directory = null, string extension = null, Action onCreate = null)
        {
            string fileName = string.Empty;
            directory ??= string.Empty;
            extension ??= string.Empty; 

            if (!string.IsNullOrWhiteSpace(label))
            {
                EditorGUILayout.LabelField(label, ScaffoldStyles.CornerLabel);
            }

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                fileName = Path.GetFileName(filePath);
            }

            EditorGUILayout.BeginHorizontal();
            {
                string iconName = isValid ? "greenLight" : "redLight";
                GUIContent icon = EditorGUIUtility.IconContent(iconName);
                EditorGUILayout.LabelField(icon, GUILayout.Width(20));

                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.TextField(fileName);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Search", GUILayout.Width(75)))
                {
                    string path = EditorUtility.OpenFilePanel("Select File", directory, extension);
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        filePath = path;
                    }
                }
                if (GUILayout.Button("Create", GUILayout.Width(75)))
                {
                    onCreate?.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();

            return filePath;
        }
    }
}