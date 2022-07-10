﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Scaffold.Launcher.Workers
{
    internal static class ProjectDefines
    {

        public static void AddDefines(string define)
        {
            List<string> defines = GetProjectDefines();
            if (defines.Contains(define))
            {
                return;
            }
            defines.Add(define);
            string defineString = string.Join(";", defines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineString);
        }


        public static void RemoveDefines(List<string> moduleDefines)
        {
            List<string> defines = GetProjectDefines();
            defines = defines.Except(moduleDefines).ToList();
            string defineString = string.Join(";", defines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineString);
        }

        public static bool CheckDefines(string define)
        {
            List<string> defines = GetProjectDefines();
            return defines.Contains(define);
        }

        public static bool CheckDefines(List<string> defines)
        {
            List<string> projectDefines = GetProjectDefines();
            return defines.Except(projectDefines).Any();
        }

        private static List<string> GetProjectDefines()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines;
        }
    }
}