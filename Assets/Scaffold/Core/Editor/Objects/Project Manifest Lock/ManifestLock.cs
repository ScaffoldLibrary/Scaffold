using System.Collections.Generic;
using System;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Core.Editor.ManifestLock
{
    /// <summary>
    /// Models a manifest.json File found in the project's package folder
    /// </summary>
    [Serializable]
    public class ManifestLock
    {
        public Dictionary<string, object> dependencies = new Dictionary<string, object>();
    }
}