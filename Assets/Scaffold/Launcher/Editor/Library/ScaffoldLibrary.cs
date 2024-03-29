﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scaffold.Core.Editor.Modules;
using System;
using UnityEditor;

namespace Scaffold.Launcher.Library
{
    [CreateAssetMenu(menuName = "Scaffold/Create Manifest")]
    public class ScaffoldLibrary : ScriptableObject
    {
        public static ScaffoldLibrary Load()
        {
#if IS_SCAFFOLD_LAUNCHER
            return AssetDatabase.LoadAssetAtPath<ScaffoldLibrary>("Assets/Scaffold/Launcher/Editor/Library/ScaffoldLibrary.asset");
#else
            return AssetDatabase.LoadAssetAtPath<ScaffoldLibrary>("Packages/com.scaffold.launcher/Editor/Library/ScaffoldLibrary.asset");
#endif
        }

        public string LibraryUrl;
        public string Hash;
        public Module Launcher = new Module();
        public Module Builder = new Module();
        public List<Module> Modules = new List<Module>();
    }
}