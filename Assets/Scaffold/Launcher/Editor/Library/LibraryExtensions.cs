using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scaffold.Core.Editor.Modules;
using System;
using UnityEditor;

namespace Scaffold.Launcher.Library
{
    internal static class LibraryExtensions
    {
        public static Module GetModule(this ScaffoldLibrary library, string name)
        {
            return library.Modules.FirstOrDefault(m => m.name == name);
        }
    }
}