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
        public static Dictionary<Module, Version> GetInstalledModules(this ScaffoldLibrary library)
        {
            List<Module> modules = library.Modules;
            List<Install> installs = library.InstalledModules;
            Dictionary<Module, Version> installedModules = new Dictionary<Module, Version>();
            foreach (var install in installs)
            {
                Module module = modules.FirstOrDefault(m => m.name == install.ModuleName);
                if (module != null)
                {
                    installedModules.Add(module, install.Version);
                }
            }
            return installedModules;
        }

        public static Module GetModule(this ScaffoldLibrary library, string name)
        {
            return library.Modules.FirstOrDefault(m => m.name == name);
        }
    }
}