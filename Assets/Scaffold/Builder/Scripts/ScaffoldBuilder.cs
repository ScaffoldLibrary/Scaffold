using System.Collections;
using UnityEngine;
using Scaffold.Core.Launcher;
using UnityEditor;

namespace Scaffold.Core.Launcher.Builder
{
    public class ScaffoldBuilder
    {

        [MenuItem("Scaffold/Build Module File")]
        public static void BuildModules()
        {
            PackageModuleBuilder _moduleBuilder = new PackageModuleBuilder();
            _moduleBuilder.BuildGraph();
        }
    }
}