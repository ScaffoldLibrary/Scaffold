using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scaffold.Builder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Builder
{
    public class ModuleManifest
    {
        public ModuleManifest(string rawManifest)
        {
            JObject manifestToken = JObject.Parse(rawManifest);
            _entries = manifestToken;
        }

        private IDictionary<string, JToken> _entries = new Dictionary<string, JToken>();

        public List<string> GetProps()
        {
            return _entries.Keys.ToList();
        }

        public T GetRawValue<T>(string key)
        {
            if (_entries.TryGetValue(key, out JToken value))
            {
                return value.ToObject<T>();
            }
            return default(T);
        }

        public object GetRawValue(string key)
        {
            if (_entries.TryGetValue(key, out JToken value))
            {
                return value;
            }
            return "";
        }

        public string GetValue(string key)
        {
            if (_entries.TryGetValue(key, out JToken value))
            {
                return value.ToString();
            }
            return "";
        }

        public void SetValue(string key, object value)
        {
            _entries[key] = JToken.FromObject(value);
        }

        public void Save(string path)
        {
            string json = JsonConvert.SerializeObject(_entries, Formatting.Indented);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }
        public bool Validate()
        {
            foreach (var entry in _entries)
            {
                string strValue = entry.Value.ToString();
                if (string.IsNullOrWhiteSpace(strValue))
                {
                    return false;
                }
            }
            return true;
        }

        public IDictionary<string, JToken> GetValues()
        {
            return _entries;
        }
    }
}