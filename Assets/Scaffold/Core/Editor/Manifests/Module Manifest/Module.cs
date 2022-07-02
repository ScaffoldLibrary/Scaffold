using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Core.Editor.Module
{
    /// <summary>
    /// Models a manifest.json File found in the project's package folder
    /// </summary>
    [Serializable]
    public class Module
    {
        public string name;
        public string displayName;
        public string description;
        public string unity;
        public string path;
        public string version;
        public ModuleAuthor author;
        public List<string> requiredModules = new List<string>();
        public List<string> requiredDefines = new List<string>();
        public List<string> installDefines = new List<string>();
    }

    [Serializable]
    public class ModuleAuthor
    {
        public string name;
        public string email;
        public string url;
    }
}
