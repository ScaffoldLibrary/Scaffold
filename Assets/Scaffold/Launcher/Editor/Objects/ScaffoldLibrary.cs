using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Launcher.Objects
{
    [CreateAssetMenu(menuName = "Scaffold/Create Manifest")]
    public class ScaffoldLibrary : ScriptableObject
    {
        public static ScaffoldLibrary Load()
        {
            if (_cachedManifest == null)
            {
                _cachedManifest = Resources.Load<ScaffoldLibrary>("Modules");
            }
            return _cachedManifest;
        }
        private static ScaffoldLibrary _cachedManifest;

        public string Hash;
        public Module Launcher = new Module();
        public List<Module> Modules = new List<Module>();

        public bool ContainsModule(string moduleName)
        {
            return Modules.Any(module => module.name == moduleName);
        }

        public Module GetModule(string moduleName)
        {
            if (ContainsModule(moduleName))
            {
                return Modules.First(package => package.name == moduleName);
            }
            else
            {
                Debug.Log($"Missing Package Reference - {moduleName}");
                return null;
            }
        }

        public List<Module> GetModuleDirectDependencies(Module module)
        {
            List<Module> modules = new List<Module>();
            foreach (string moduleName in module.requiredModules)
            {
                if (!ContainsModule(moduleName))
                {
                    Debug.Log($"Scaffold Manifest is missing the required module {moduleName}, please update your launcher");
                    continue;
                }
                modules.Add(GetModule(moduleName));
            }
            return modules;
        }

        public List<Module> GetModuleDependencies(Module module)
        {
            List<Module> dependencies = new List<Module>();
            Stack<Module> stack = new Stack<Module>();
            stack.Push(module);
            while (stack.Count > 0)
            {
                Module currentModule = stack.Pop();
                dependencies.Add(currentModule);
                foreach (string moduleName in module.requiredModules)
                {
                    if (!ContainsModule(moduleName))
                    {
                        continue;
                    }

                    if (dependencies.Any(m => m.name == moduleName) || stack.Any(m => m.name == moduleName))
                    {
                        continue;
                    }

                    Module dependency = GetModule(moduleName);
                    stack.Push(dependency);
                }
            }

            return dependencies;
        }

        public void AddModule(Module module)
        {
            if (ContainsModule(module.name))
            {
                return;
            }

            Modules.Add(module);
        }
    }
}