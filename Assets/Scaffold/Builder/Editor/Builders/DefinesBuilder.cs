using Newtonsoft.Json.Linq;
using Scaffold.Builder.Utilities;
using Scaffold.Core.Editor.Modules;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scaffold.Builder.FileBuilders
{
    internal class DefinesBuilder : ModuleBuilder
    {
        public DefinesBuilder(BuilderConfigs configs) : base(configs)
        {
            _module = configs.Module;
        }

        private Module _module;

        public override bool Validate()
        {
            return _module.requiredDefines != null && _config.Assemblies != null;
        }

        public override void Build()
        {
            List<string> defines = _module.requiredDefines;
            List<string> assemblies = _config.Assemblies;
            AddDefinesToAssemblies(assemblies, defines);
        }

        public void AddDefinesToAssemblies(List<string> assemblies, List<string> defines)
        {
            foreach (string assemblyPath in assemblies)
            {
                string assemblyRaw = File.ReadAllText(assemblyPath);
                JObject assembly = JObject.Parse(assemblyRaw);
                if (!assembly.ContainsKey("defineConstraints") || assembly["defineConstraints"] == null)
                {
                    JToken definesToken = JToken.FromObject(defines);
                    assembly["defineConstraints"] = definesToken;
                }
                else
                {
                    JToken definesToken = assembly["defineConstraints"];
                    List<string> rawDefines = definesToken.ToObject<List<string>>();
                    rawDefines = rawDefines.Union(defines).ToList();
                    assembly["defineConstraints"] = JToken.FromObject(rawDefines);
                }
                File.WriteAllText(assemblyPath, assembly.ToString());
            }
        }
    }
}