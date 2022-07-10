using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Builder.Editor;
using Scaffold.Builder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scaffold.Core.Editor.Modules;
using Scaffold.Builder.FileBuilders;

namespace Scaffold.Builder
{
    public static class ScaffoldBuilder
    {
        public static BuilderConfigs Config
        {
            get
            {
                if (_config == null)
                {
#if !IS_SCAFFOLD_BUILDER
                    _config = AssetDatabase.LoadAssetAtPath<BuilderConfigs>("Packages/com.scaffold.builder/Editor/Config/BuilderConfigs.asset");
#else
                    _config = AssetDatabase.LoadAssetAtPath<BuilderConfigs>("Assets/Scaffold/Builder/Editor/Config/BuilderConfigs.asset");
#endif
                }
                return _config;
            }
        }
        private static BuilderConfigs _config;

        private static bool _lastValidation;

        [MenuItem("Scaffold/Builder/Build", priority = 0)]
        public static void Build()
        {
            BuilderWindow.OpenBuilder();
        }

        [MenuItem("Scaffold/Builder/Quick Build %#Q", priority = 1)]
        public static void QuickBuild()
        {
            if(ValidateBuilders(out List<ModuleBuilder> builders))
            {
                foreach(ModuleBuilder builder in builders)
                {
                    builder.Build();
                }
            }
        }

        [MenuItem("Scaffold/Builder/Quick Build %#Q", true)]
        private static bool ValidateQuickBuild()
        {
            return _lastValidation;
        }

        private static bool ValidateBuilders(out List<ModuleBuilder> builders)
        {
            builders = GetBuilders();
            foreach (ModuleBuilder builder in builders)
            {
                if (!builder.Validate())
                {
                    _lastValidation = false;
                    return false;
                }
            }
            _lastValidation = true;
            return true;
        }

        private static List<ModuleBuilder> GetBuilders()
        {
            return new List<ModuleBuilder>()
            {
                new AssemblyBuilder(Config),
                new DefinesBuilder(Config),
                new InstallerBuilder(Config)
            };
        }
    }
}
