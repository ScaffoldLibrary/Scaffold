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
            return dependencies.Where(d => d.Contains("scaffold") && !d.Contains("scaffold.builder") && !d.Contains("scaffold.launcher")).ToList();
        }
    }
}