using System.Collections;
using UnityEngine;
using Scaffold.Launcher.Objects;
using System;
using System.Threading.Tasks;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Launcher.Utilities
{
    internal static class ModuleFetcher
    {
        private static string ApiUrl = "https://4gy3gio67e2eiaea4qrajylonm0tukgv.lambda-url.sa-east-1.on.aws/";

        public static async Task<Module> GetModule(string moduleKey)
        {
            GetModuleRequest request = new GetModuleRequest(moduleKey);
            Module module = await HttpHandler.Post<Module>(ApiUrl, request);
            return module;
        }

        public static async Task<ScaffoldLibrary> GetManifest()
        {
            GetManifestRequest request = new GetManifestRequest();
            ScaffoldLibrary manifest = await HttpHandler.Post<ScaffoldLibrary>(ApiUrl, request);
            return manifest;
        }

        private class GetModuleRequest
        {
            public GetModuleRequest(string module)
            {
                moduleKey = module;
            }

            public string requestType = "get";
            public string moduleKey;
        }

        private class GetManifestRequest
        {
            public string requestType = "getAll";
        }
    }
}