using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Builder.Utilities
{
    internal static partial class ModuleUploader
    {
        public static async void UploadModule(Module module, Credentials credential, Action<string> Result = null)
        {
            if (credential == null)
            {
                Result?.Invoke("Invalid Credentials");
                return;
            }

            if (module == null)
            {
                Result?.Invoke("Invalid Module");
                return;
            }

            SetModuleRequest request = new SetModuleRequest(module, credential.apiKey);
            string result = await HttpHandler.Post(credential.apiURL, request);
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