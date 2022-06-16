using Scaffold.Launcher.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            List<ScaffoldModule> modules = _projectManifest.FilterScaffoldModules();
            missingModules = modules.SelectMany(m => m.dependencies)
                                    .Distinct()
                                    .Where(d => !modules.Any(m => m.Key == d))
                                    .Where(d => _scaffoldManifest.ContainModule(d))
                                    .Select(d => _scaffoldManifest.GetPackage(d))
                                    .ToList();

            return missingModules != null && missingModules.Count > 0;
        }
    }
}
        //public List<PackagePath> GetPackageDependencies(PackagePath package)
        //{
        //    if (!Modules.Contains(package))
        //    {
        //        return null;
        //    }

        //    List<PackagePath> dependencies = new List<PackagePath>();
        //    foreach (string packageName in package.dependencies)
        //    {
        //        PackagePath dependency = GetPackage(packageName);
        //        if (dependency == null)
        //        {
        //            continue;
        //        }

        //        if (dependencies.Contains(dependency))
        //        {
        //            continue;
        //        }

        //        dependencies.Add(dependency);
        //    }

        //    return dependencies;
        //}