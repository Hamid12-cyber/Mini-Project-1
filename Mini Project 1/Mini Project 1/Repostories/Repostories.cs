using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mini_Project_1.Repostories
{
    internal class Repostories<T>
    {
        public readonly string _path;

        public Repostories(string relativePath)
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;

            string targetKey = "Mini projecthm";
            int keyIndex = relativePath.IndexOf(targetKey, StringComparison.OrdinalIgnoreCase);

            if (keyIndex != -1)
            {
                string cleanRelativePath = relativePath.Substring(keyIndex);

                int dirIndex = currentDir.IndexOf(targetKey, StringComparison.OrdinalIgnoreCase);

                if (dirIndex != -1)
                {
                    string rootPath = currentDir.Substring(0, dirIndex);
                    _path = Path.Combine(rootPath, cleanRelativePath);
                }
                else
                {
                    _path = Path.Combine(currentDir, relativePath);
                }
            }
            else
            {
                _path = Path.Combine(currentDir, relativePath);
            }

            Console.WriteLine("PATH: " + _path);

            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        }

        public virtual void Serialize(List<T> items)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };
            File.WriteAllText(_path, JsonConvert.SerializeObject(items, settings));
        }

        public virtual List<T> Deserialize()
        {
            if (!File.Exists(_path))
                return new List<T>();

            string json = File.ReadAllText(_path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            if (list.Count > 0)
                SyncFromList(list);

            return list;
        }

        protected virtual void SyncFromList(List<T> list) { }
    }
}