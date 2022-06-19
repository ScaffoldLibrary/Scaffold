using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scaffold.Launcher.PackageHandler
{
    [CreateAssetMenu(menuName = "Scaffold/Create Manifest")]
    public class ScaffoldManifest : ScriptableObject
    {
        public static ScaffoldManifest Fetch()
        {
            if (_cachedManifest == null)
            {
                _cachedManifest = Resources.Load<ScaffoldManifest>("Modules");
            }
            return _cachedManifest;
        }
        private static ScaffoldManifest _cachedManifest;

        public ScaffoldModule Launcher = new ScaffoldModule();
        public List<ScaffoldModule> Modules = new List<ScaffoldModule>();

        public bool ContainModule(string moduleKey)
        {
            return Modules.Any(package => package.Key == moduleKey);
        }

        public ScaffoldModule GetModule(string moduleKey)
        {
            if (ContainModule(moduleKey))
            {
                return Modules.First(package => package.Key == moduleKey);
            }
            else
            {
                Debug.Log($"Missing Package Reference - {moduleKey}");
                return null;
            }
        }

        public List<ScaffoldModule> GetModuleDependencies(ScaffoldModule module)
        {
            List<ScaffoldModule> modules = new List<ScaffoldModule>();
            foreach (string moduleKey in module.Dependencies)
            {
                if (!ContainModule(moduleKey))
                {
                    Debug.Log($"Scaffold Manifest is missing the required module {moduleKey}");
                    continue;
                }
                modules.Add(GetModule(moduleKey));
            }
            return modules;
        }
    }
}