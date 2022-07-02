using Newtonsoft.Json.Linq;
using Scaffold.Builder.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scaffold.Builder.FileBuilders
{
    internal class DefinesBuilder: ModuleBuilder
    {
        public DefinesBuilder(BuilderConfigs configs) : base(configs)
        {

        }

        public override bool Validate()
        {
            return _config.RequiredDefines != null && _config.Assemblies != null;
        }

        public override void Build()
        {
            List<string> defines = _config.RequiredDefines;
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
                    rawDefines.AddRange(defines);
                    assembly["defineConstraints"] = JToken.FromObject(rawDefines);
                }
                File.WriteAllText(assemblyPath, assembly.ToString());
            }
        }
    }
}