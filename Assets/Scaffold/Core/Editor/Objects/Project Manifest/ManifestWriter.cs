using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scaffold.Core.Editor.Manifest
{
    /// <summary>
    /// Writes a <c>Manifest</c> object to disk
    /// </summary>
    public class ManifestWriter
    {
        public ManifestWriter(string path)
        {
            _manifestPath = path;
        }

        private string _manifestPath;

        public void Save(Manifest manifest)
        {
            string rawManifest = JsonConvert.SerializeObject(manifest);
            File.WriteAllText(_manifestPath, rawManifest);
        }
    }
}
