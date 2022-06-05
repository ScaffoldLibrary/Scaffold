using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scaffold.Core.Launcher.ModuleHandler
{
    public class ModuleReader
    {
        public ModuleReader(string path)
        {
            _moduleFilePath = path;
        }

        private string _moduleFilePath;

        public IDictionary<string, string> ReadFile()
        {
            string text = File.ReadAllText(_moduleFilePath);
            JObject jObj = JObject.Parse(text);
            return jObj.ToObject<IDictionary<string, string>>();
        }

        public bool LookForKey(string key, out string value)
        {
            var file = ReadFile();
            if(file[key] != null)
            {
                value = file[key];
                return true;
            }
            value = null;
            return false;
        }
    }
}