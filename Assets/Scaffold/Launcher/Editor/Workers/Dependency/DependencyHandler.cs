using Scaffold.Launcher.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scaffold.Launcher.Library;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;

namespace Scaffold.Launcher.Workers
{
    public class DependencyHandler
    {
        public DependencyHandler(ScaffoldLibrary library, Manifest manifest)
        {
            _library = library;
            _manifest = manifest;
        }

        private ScaffoldLibrary _library;
        private Manifest _manifest;

        public bool ValidateDependencies()
        {
            return ValidateDependencies(out List<Module> modules);
        }

        public bool ValidateDependencies(out List<Module> missingModules)
        {
            List<Module> installedModules = _manifest.GetScaffoldDependencies().Select(d => _library.GetModule(d)).ToList();
            missingModules = installedModules.SelectMany(m => m.requiredModules)
                                                        .Distinct()
                                                        .Select(m => _library.GetModule(m))
                                                        .Except(installedModules)
                                                        .ToList();

            return missingModules != null && missingModules.Count > 0;
        }

        public bool CheckForDependingModules(Module module, out List<Module> modules)
        {
            List<Module> installedModules = _manifest.GetScaffoldDependencies().Select(d => _library.GetModule(d)).ToList();
            modules = installedModules.Where(m => m != null && GetModuleDependencies(m, false).Contains(module)).ToList();
            return modules.Count > 0;
        }

        public List<Module> GetModuleDependencies(Module module, bool includeSelf)
        {
            List<Module> modules = new List<Module>();
            Queue<Module> pendingModules = new Queue<Module>();
            pendingModules.Enqueue(module);

            while(pendingModules.Count > 0)
            {
                Module current = pendingModules.Dequeue();
                if (modules.Contains(current))
                {
                    continue;
                }

                modules.Add(current);
                var dependencies = GetDirectModuleDependencies(current);
                foreach(Module dep in dependencies)
                {
                    if(!modules.Contains(dep) && !pendingModules.Contains(dep))
                    {
                        pendingModules.Enqueue(dep);
                    }
                }
            }

            if (!includeSelf)
            {
                modules.Remove(module);
            }
            return modules;
        }

        private List<Module> GetDirectModuleDependencies(Module module)
        {
            return _library.Modules.Where(m => module.requiredModules.Contains(m.name)).ToList();
        }
    }
}
