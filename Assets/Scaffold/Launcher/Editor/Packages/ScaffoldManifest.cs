using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scaffold.Launcher.PackageHandler
{
    public class ScaffoldManifest : ScriptableObject
    {
        public static ScaffoldManifest Fetch()
        {
            if (mModules == null)
            {
                mModules = Resources.Load<ScaffoldManifest>("Modules");
            }
            return mModules;
        }

        private static ScaffoldManifest mModules;

        public ScaffoldModule Launcher = new ScaffoldModule();
        public List<ScaffoldModule> Modules = new List<ScaffoldModule>();

        public bool ContainModule(string packageName)
        {
            return Modules.Any(package => package.Key == packageName);
        }

        public ScaffoldModule GetPackage(string packageName)
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