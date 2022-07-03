using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Scaffold.Builder.Utilities
{
    public class Credentials
    {
        public string apiKey;
        public string apiURL;

        public static Credentials Fetch(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Debug.Log("Invalid Credentials Path");
                return null;
            }

            string rawCredentials = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Credentials>(rawCredentials);
        }
    }
}