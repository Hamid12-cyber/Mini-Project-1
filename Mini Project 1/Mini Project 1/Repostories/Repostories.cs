using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Repostories
{
    internal class Repostories<T>
    {
        public readonly string _path;

        public Repostories(string path)
        {
            _path = path;
        }
        public void Serialize(List<T> items)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            string json = JsonConvert.SerializeObject(items, settings);
            using (StreamWriter sw = new StreamWriter(_path))
            {
                sw.Write(json);
            }


        }
        public List<T> Deserialize()
        {
            if (!File.Exists(_path))
                return new List<T>();
            string json;

            using (StreamReader sr = new StreamReader(_path))
            {
                json = sr.ReadToEnd();
            }
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            List<T> list = JsonConvert.DeserializeObject<List<T>>(json);

            if (list != null && list.Count > 0)
                SyncFromList(list);
            return list ?? new List<T>();


        }
          protected virtual void SyncFromList(List<T> list) { }
    }
}
