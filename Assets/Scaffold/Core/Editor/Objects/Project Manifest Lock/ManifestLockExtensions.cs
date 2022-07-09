using System.Collections;
using UnityEngine;

namespace Scaffold.Core.Editor.ManifestLock
{
    public static class ManifestLockExtensions
    {
        public static bool Contains(this ManifestLock manifest, string moduleName)
        {
            return manifest.dependencies.ContainsKey(moduleName);
        }

        public static void Remove(this ManifestLock manifest, string moduleName)
        {
            if (!manifest.Contains(moduleName))
            {
                return;
            }

            manifest.dependencies.Remove(moduleName);
        }
    }
}