using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Scaffold.Builder.Utilities
{
    internal static partial class ModuleUploader
    {
        public static async void UploadModule(BuilderConfigs config, Action<string> Result = null)
        {
            SetModuleRequest request = new SetModuleRequest(config.Manifest, config.Credential.apiKey);
            Debug.Log(JsonConvert.SerializeObject(request));
            string result = await HttpFetcher.Fetch(config.Credential.apiURL, request);
            Result?.Invoke(result);
        }

        private class SetModuleRequest
        {
            public SetModuleRequest(ModuleManifest module, string key)
            {
                this.module = module.GetValues();
                this.key = key;
            }

            public string requestType = "set";
            public IDictionary<string, JToken> module;
            public string key;
        }
    }
}