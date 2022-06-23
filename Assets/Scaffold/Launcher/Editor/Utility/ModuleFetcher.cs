using System.Collections;
using UnityEngine;
using Scaffold.Launcher.Objects;
using System;
using System.Threading.Tasks;

namespace Scaffold.Launcher.Utilities
{
    internal static class ModuleFetcher
    {
        private static string ApiUrl = "https://4gy3gio67e2eiaea4qrajylonm0tukgv.lambda-url.sa-east-1.on.aws/";

        public static async Task<ScaffoldModule> GetModule(string moduleKey)
        {
            GetModuleRequest request = new GetModuleRequest(moduleKey);
            ScaffoldModule module = await HttpFetcher.Fetch<ScaffoldModule>(ApiUrl, request);
            return module;
        }

        public static async Task<ScaffoldManifest> GetManifest()
        {
            GetManifestRequest request = new GetManifestRequest();
            ScaffoldManifest manifest = await HttpFetcher.Fetch<ScaffoldManifest>(ApiUrl, request);
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