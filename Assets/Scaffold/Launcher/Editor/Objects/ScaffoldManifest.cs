﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scaffold.Launcher.Objects
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

        public string Hash;
        public ScaffoldModule Launcher = new ScaffoldModule();
        public List<ScaffoldModule> Modules = new List<ScaffoldModule>();

        public bool ContainsModule(string moduleKey)
        {
            return Modules.Any(module => module.Key == moduleKey);
        }

        public ScaffoldModule GetModule(string moduleKey)
        {
            if (ContainsModule(moduleKey))
            {
                return Modules.First(package => package.Key == moduleKey);
            }
            else
            {
                Debug.Log($"Missing Package Reference - {moduleKey}");
                return null;
            }
        }

        public List<ScaffoldModule> GetModuleDirectDependencies(ScaffoldModule module)
        {
            List<ScaffoldModule> modules = new List<ScaffoldModule>();
            foreach (string moduleKey in module.Dependencies)
            {
                if (!ContainsModule(moduleKey))
                {
                    Debug.Log($"Scaffold Manifest is missing the required module {moduleKey}");
                    continue;
                }
                modules.Add(GetModule(moduleKey));
            }
            return modules;
        }

        public List<ScaffoldModule> GetModuleDependencies(ScaffoldModule module)
        {
            List<ScaffoldModule> dependencies = new List<ScaffoldModule>();
            Stack<ScaffoldModule> stack = new Stack<ScaffoldModule>();
            stack.Push(module);
            while (stack.Count > 0)
            {
                ScaffoldModule currentModule = stack.Pop();
                dependencies.Add(currentModule);
                foreach (string moduleKey in module.Dependencies)
                {
                    if (!ContainsModule(moduleKey))
                    {
                        continue;
                    }

                    if (dependencies.Any(m => m.Key == moduleKey) || stack.Any(m => m.Key == moduleKey))
                    {
                        continue;
                    }

                    ScaffoldModule dependency = GetModule(moduleKey);
                    stack.Push(dependency);
                }
            }

            return dependencies;
        }

        public void AddModule(ScaffoldModule module)
        {
            if (ContainsModule(module.Key))
            {
                return;
            }

            Modules.Add(module);
        }
    }
}