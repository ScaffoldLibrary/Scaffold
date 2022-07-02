using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Scaffold.Builder.FileBuilders;
using System.Text.RegularExpressions;
using Scaffold.Builder.Utilities;

namespace Scaffold.Builder
{
    public abstract class ModuleBuilder
    {
        public ModuleBuilder(BuilderConfigs config)
        {
            _config = config;
        }

        protected BuilderConfigs _config;

        public abstract bool Validate();

        public abstract void Build();
    }
}
