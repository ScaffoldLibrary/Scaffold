using Scaffold.Launcher.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scaffold.Launcher.Objects;

namespace Scaffold.Launcher.PackageHandler
{
    public class DependencyValidator
    {
        public DependencyValidator(ScaffoldManifest scaffoldManifest, ProjectManifest projectManifest)
        {
            _scaffoldManifest = scaffoldManifest;
            _projectManifest = projectManifest;
        }

        private ScaffoldManifest _scaffoldManifest;
        private ProjectManifest _projectManifest;

        public bool ValidateDependencies()
        {
            return ValidateDependencies(out List<ScaffoldModule> modules);
        }

        public bool ValidateDependencies(out List<ScaffoldModule> missingModules)
        {
            List<ScaffoldModule> modules = _projectManifest.FilterScaffoldModules(_scaffoldManifest);
            missingModules = modules.SelectMany(m => m.Dependencies)
                                    .Distinct()
                                    .Where(d => !modules.Any(m => m.Key == d))
                                    .Where(d => _scaffoldManifest.ContainsModule(d))
                                    .Select(d => _scaffoldManifest.GetModule(d))
                                    .ToList();

            return missingModules != null && missingModules.Count > 0;
        }

        public bool CheckForDependingModules(ScaffoldModule module, out List<ScaffoldModule> modules)
        {
            List<ScaffoldModule> installedModules = _projectManifest.FilterScaffoldModules(_scaffoldManifest);
            modules = installedModules.Where(m => _scaffoldManifest.GetModuleDependencies(m).Contains(module)).ToList();
            return modules.Count > 0;
        }

        public bool CheckForDepencyDependers(ScaffoldModule module)
        {
            List<ScaffoldModule> moduleDependencies = _scaffoldManifest.GetModuleDependencies(module);
            List<ScaffoldModule> installedModules = _projectManifest.FilterScaffoldModules(_scaffoldManifest);
            List<ScaffoldModule> projectDependencies = installedModules.SelectMany(m => _scaffoldManifest.GetModuleDependencies(m))
                                                                       .Distinct()
                                                                       .ToList();
            return moduleDependencies.Except(projectDependencies).Any();
        }
    }
}
