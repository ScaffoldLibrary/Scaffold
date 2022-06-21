using System.Collections;
using UnityEngine;
using Scaffold.Launcher.Objects;
using System;
using System.Threading.Tasks;

namespace Scaffold.Launcher.Utilities
{
    internal static class ModuleFetcher
    {
        private static string ApiUrl = "https://e227iwvnp2dov6ddpl3ujveyyi0vahcu.lambda-url.us-east-1.on.aws/";

        public static async Task<ScaffoldModule> GetModule(string moduleName)
        {
            GetModuleRequest request = new GetModuleRequest(moduleName);
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
                moduleName = module;
            }

            public string requestType = "get";
            public string moduleName;
        }

        private class GetManifestRequest
        {
            public string requestType = "getAll";
        }
    }
}