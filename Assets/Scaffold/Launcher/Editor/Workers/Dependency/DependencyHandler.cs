using Scaffold.Launcher.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scaffold.Launcher.Objects;
using Scaffold.Core.Editor.Modules;
using Scaffold.Core.Editor.Manifest;

namespace Scaffold.Launcher.Workers
{
    public class DependencyHandler
    {
        public DependencyHandler(ScaffoldLibrary scaffoldManifest, Manifest projectManifest)
        {
            _scaffoldManifest = scaffoldManifest;
            _projectManifest = projectManifest;
        }

        private ScaffoldLibrary _scaffoldManifest;
        private Manifest _projectManifest;

        public bool ValidateDependencies()
        {
            return ValidateDependencies(out List<Module> modules);
        }

        public bool ValidateDependencies(out List<Module> missingModules)
        {
            List<Module> installedModules = new List<Module>();
            //missingModules = installedModules.SelectMany(m => m.requiredModules)
            //                        .Distinct()
            //                        .Where(d => !installedModules.Any(m => m.name == d))
            //                        .Where(d => _scaffoldManifest.ContainsModule(d))
            //                        .Select(d => _scaffoldManifest.GetModule(d))
            //                        .ToList();
            missingModules = null;

            return missingModules != null && missingModules.Count > 0;
        }

        public bool CheckForDependingModules(Module module, out List<Module> modules)
        {
            List<Module> installedModules = new List<Module>();
            modules = installedModules.Where(m => GetModuleDependencies(m, false).Contains(module)).ToList();
            return modules.Count > 0;
        }

        public List<Module> GetModuleDependencies(Module module, bool includeSelf)
        {
            return null;
        }
    }
}
