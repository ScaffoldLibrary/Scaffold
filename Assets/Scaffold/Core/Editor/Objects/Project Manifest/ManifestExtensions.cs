using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scaffold.Core.Editor.Manifest
{
    public static class ManifestExtensions
    {
        public static List<string> GetScaffoldDependencies(this Manifest manifest)
        {
            List<string> dependencies = manifest.dependencies.Select(kp => kp.Key).ToList();
            return dependencies.Where(d => d.Contains("scaffold") && !d.Contains("scaffold.builder") && !d.Contains("scaffold.launcher") && !d.Contains("scaffold.core")).ToList();
        }

        public static bool Contains(this Manifest manifest, string moduleName)
        {
            return manifest.dependencies.ContainsKey(moduleName);
        }

        public static void Add(this Manifest manifest, string moduleName, string modulePath)
        {
            if (manifest.Contains(moduleName))
            {
                return;
            }

            manifest.dependencies.Add(moduleName, modulePath);
        }

        public static void Remove(this Manifest manifest, string moduleName)
        {
            if (!manifest.Contains(moduleName))
            {
                return;
            }

            manifest.dependencies.Remove(moduleName);
        }
    }
}