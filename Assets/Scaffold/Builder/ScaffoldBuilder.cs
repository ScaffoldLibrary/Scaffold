using System.Collections;
using UnityEngine;
using Scaffold.Core.Launcher;
using UnityEditor;

namespace Scaffold.Core.Launcher.Builder
{
    public class ScaffoldBuilder
    {
        private PackageModuleBuilder _moduleBuilder = new PackageModuleBuilder();
        
        [MenuItem("Scaffold/Build Module File")]
        public void BuildModules()
        {
            _moduleBuilder.BuildGraph();
        }
    }
}