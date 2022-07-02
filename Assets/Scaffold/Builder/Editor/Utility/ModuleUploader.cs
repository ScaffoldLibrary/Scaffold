using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Module;

namespace Scaffold.Builder.Utilities
{
    internal static partial class ModuleUploader
    {
        public static async void UploadModule(BuilderConfigs config, Action<string> Result = null)
        {
            SetModuleRequest request = new SetModuleRequest(config.Module, config.Credential.apiKey);
            string result = await HttpHandler.Post(config.Credential.apiURL, request);
            Result?.Invoke(result);
        }

        private class SetModuleRequest
        {
            public SetModuleRequest(Module module, string key)
            {
                this.module = module;
                this.key = key;
            }

            public string requestType = "set";
            public Module module;
            public string key;
        }
    }
}