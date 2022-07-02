using System.Collections.Generic;
using System;

namespace Scaffold.Core.Editor.Manifest
{
    /// <summary>
    /// Models a manifest.json File found in the project's package folder
    /// </summary>
    [Serializable]
    public class Manifest
    {
        public Dictionary<string, string> dependencies = new Dictionary<string, string>();
    }
}