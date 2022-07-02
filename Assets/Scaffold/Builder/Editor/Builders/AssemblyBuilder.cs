using Scaffold.Builder.Utilities;
using System.IO;
using UnityEngine;
using Scaffold.Core.Editor.Module;

namespace Scaffold.Builder.FileBuilders
{
    public class AssemblyBuilder : ModuleBuilder
    {
        public AssemblyBuilder(BuilderConfigs config) : base(config)
        {

        }

        public override void Build()
        {
            string assembly = GetRawAssembly();
            WriteAssembly(assembly);
        }

        public string GetRawAssembly()
        {
            string assembly = ReadTemplateAssembly();
            return EditTemplateAssembly(assembly);
        }

        private string ReadTemplateAssembly()
        {
            string template = _config.TemplateAssembly;
            return template;
        }

        private string EditTemplateAssembly(string assembly)
        {
            assembly = assembly.Replace("ModuleName", _config.Module.GetPascalName());
            return assembly;
        }

        public void WriteAssembly(string assembly)
        {
            if (string.IsNullOrEmpty(assembly))
            {
                return;
            }

            File.WriteAllText(_config.InstallerAssemblyPath, assembly);
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_config.InstallerAssemblyPath))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_config.TemplateAssembly))
            {
                return false;
            }

            return true;
        }


    }
}