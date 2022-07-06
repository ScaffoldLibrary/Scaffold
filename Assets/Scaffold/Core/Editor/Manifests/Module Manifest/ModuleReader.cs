using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scaffold.Core.Editor.Modules
{
    /// <summary>
    /// Reads and Create a <c>Module</c> object from disk
    /// </summary>
    public class ModuleReader
    {
        public ModuleReader(string path)
        {
            _modulePath = path;
        }

        private string _modulePath;
        
        public Module GetModule()
        {
            string rawModule = GetRawModule();
            return JsonConvert.DeserializeObject<Module>(rawModule);
        }

        private string GetRawModule()
        {
            return File.ReadAllText(_modulePath);
        }
    }
}
