using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scaffold.Launcher.PackageHandler
{
    public class ScaffoldManifest : ScriptableObject
    {
        public PackagePath Launcher = new PackagePath();
        public List<PackagePath> Modules = new List<PackagePath>();

        public bool ContainModule(string packageName)
        {
            return Modules.Any(package => package.Key == packageName);
        }

        public PackagePath GetPackage(string packageName)
        {
            if (ContainModule(packageName))
            {
                return Modules.First(package => package.Key == packageName);
            }
            else
            {
                Debug.Log($"Missing Package Reference - {packageName}");
                return null;
            }
        }
    }
}