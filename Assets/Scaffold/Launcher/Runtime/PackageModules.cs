using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scaffold.Core.Launcher
{
    public class PackageModules : ScriptableObject
    {
        public List<PackagePath> packages = new List<PackagePath>();
        public bool ContainModule(string packageName)
        {
            return packages.Any(package => package.key == packageName);
        }

        //public string GetModuleDisplayName(string packageName)
        //{

        //    if (ContainModule(packageName))
        //    {
        //        return packages.First(package => package.name == packageName).displayName;
        //    }
        //    else
        //    {
        //        return $"Missing Package Reference - {packageName}";
        //    }
        //}

        public PackagePath GetPackage(string packageName)
        {
            if (ContainModule(packageName))
            {
                return packages.First(package => package.key == packageName);
            }
            else
            {
                Debug.Log($"Missing Package Reference - {packageName}");
                return null;
            }
        }

        public List<PackagePath> GetPackageDependencies(PackagePath package)
        {
            if (!packages.Contains(package))
            {
                return null;
            }

            List<PackagePath> dependencies = new List<PackagePath>();
            foreach (string packageName in package.dependencies)
            {
                PackagePath dependency = GetPackage(packageName);
                if(dependency == null)
                {
                    continue;
                }

                if (dependencies.Contains(dependency))
                {
                    continue;
                }

                dependencies.Add(dependency);
            }

            return dependencies;
        }
    }
}