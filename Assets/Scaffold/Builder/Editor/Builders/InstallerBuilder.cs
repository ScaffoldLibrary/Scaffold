using Scaffold.Builder.Utilities;
using Scaffold.Core.Editor.Module;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scaffold.Builder.FileBuilders
{
    internal class InstallerBuilder : ModuleBuilder
    {
        public InstallerBuilder(BuilderConfigs config) : base(config)
        {
            _config = config;
            _module = config.Module;
        }

        private Module _module;

        public override void Build()
        {
            string installer = GetRawInstaller();
            WriteInstaller(installer);
        }

        internal string GetRawInstaller()
        {
            string installer = GetInstallerTemplate();
            installer = ReplaceModuleName(installer);
            installer = ReplaceRequiredDefines(installer);
            installer = ReplaceInstallDefines(installer);
            return installer;
        }

        private string GetInstallerTemplate()
        {
            string template = _config.TemplateInstaller;
            if (string.IsNullOrEmpty(template))
            {
                Debug.Log("Template Installer is empty, make sure you have setup properly");
            }
            return template;
        }

        private string ReplaceModuleName(string script)
        {
            string name = _module.GetPascalName();
            script = script.Replace("MODULENAME", name.ToUpperInvariant());
            script = script.Replace("ModuleName", name);
            return script;
        }

        private string ReplaceRequiredDefines(string script)
        {
            List<string> defines = _module.requiredDefines;
            if (defines.Count <= 0)
            {
                script = script.Replace("\"#REQUIREMENTS#\"", "");
            }
            else
            {
                string requirements = string.Join("\", \"", defines);
                script = script.Replace("#REQUIREMENTS#", requirements);
            }

            return script;
        }

        private string ReplaceInstallDefines(string script)
        {
            List<string> defines = _module.installDefines;
            if (defines.Count <= 0)
            {
                script = script.Replace("\"#INSTALLS#\"", "");
            }
            else
            {
                string installs = string.Join("\", \"", defines);
                script = script.Replace("#INSTALLS#", installs);
            }

            return script;
        }

        public void WriteInstaller(string script)
        {
            if (string.IsNullOrWhiteSpace(script))
            {
                return;
            }

            File.WriteAllText(_config.InstallerPath, script);
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_config.InstallerPath)) return false;
            if (string.IsNullOrWhiteSpace(_config.TemplateInstaller)) return false;
            if (_module.requiredDefines == null) return false;
            if (_module.installDefines == null) return false;
            return true;
        }
    }
}