using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Core.Launcher
{
    public class PackageModules : ScriptableObject
    {
        public List<PackagePath> packages = new List<PackagePath>();
    }
}