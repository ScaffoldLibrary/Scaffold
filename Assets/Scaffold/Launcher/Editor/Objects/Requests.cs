using System.Collections;
using UnityEngine;

namespace Scaffold.Launcher.Requests
{
    public class GetModuleRequest
    {
        public GetModuleRequest(string module)
        {
            moduleName = module;
        }

        public string requestType = "get";
        public string moduleName;
    }

    public class GetAllModulesRequest
    {
        public string requestType = "getAll";
    }
}