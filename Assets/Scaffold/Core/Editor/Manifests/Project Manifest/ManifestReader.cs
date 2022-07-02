using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Scaffold.Core.Editor.Manifest
{
    /// <summary>
    /// Reads and Create a <c>Manifest</c> object from disk
    /// </summary>
    public class ManifestReader
    {
        public ManifestReader(string path)
        {
            _manifestPath = path;
        }

        private string _manifestPath;

        public Manifest GetManifest()
        {
            string rawManifest = GetRawManfest();
            return JsonConvert.DeserializeObject<Manifest>(rawManifest);
        }

        private string GetRawManfest()
        {
            return File.ReadAllText(_manifestPath);
        }
    }
}