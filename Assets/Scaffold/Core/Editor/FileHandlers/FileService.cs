using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scaffold.Core.Editor
{
    public class FileService
    {
        private Dictionary<Type, string> _filePaths = new Dictionary<Type, string>();

        public T Read<T>(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("Trying to read a invalid file path");
                return default(T);
            }

            string content = File.ReadAllText(path);
            T obj = JsonConvert.DeserializeObject<T>(content);
            _filePaths[typeof(T)] = path;
            return default(T);
        }

        public void Save<T>(T instance)
        {
            Type type = typeof(T);
            string path = _filePaths[type];
            Save(instance, path);
        }

        public void Save<T>(T instance, string path)
        {
            string json = JsonConvert.SerializeObject(instance, Formatting.Indented);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }
    }
}