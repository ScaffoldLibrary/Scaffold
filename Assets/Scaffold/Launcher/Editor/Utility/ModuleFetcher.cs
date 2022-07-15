using System.Collections;
using UnityEngine;
using Scaffold.Launcher.Library;
using System;
using System.Threading.Tasks;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;

namespace Scaffold.Launcher.Utilities
{
    internal static class ModuleFetcher
    {
        public static async Task<Module> GetModule(string moduleKey, string apiUrl)
        {
            GetModuleRequest request = new GetModuleRequest(moduleKey);
            Module module = await HttpHandler.Post<Module>(apiUrl, request);
            return module;
        }

        public static async Task<ScaffoldLibrary> GetManifest(string apiUrl)
        {
            GetManifestRequest request = new GetManifestRequest();
            ScaffoldLibrary manifest = await HttpHandler.Post<ScaffoldLibrary>(apiUrl, request);
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