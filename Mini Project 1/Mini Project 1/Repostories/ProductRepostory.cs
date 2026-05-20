using Mini_Project_1.Enums;
using Mini_Project_1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mini_Project_1.Repostories
{
    internal class ProductRepostory : Repostories<Product>
    {
        public ProductRepostory() : base(@"Mini projecthm\Mini Project 1\Mini Project 1\Data\Product.json") { }

        public override void Serialize(List<Product> products)
        {
            var grouped = new Dictionary<string, List<Product>>
            {
                ["Elektronika"] = products.Where(p => p.Category == ProductCategory.Elektronika).ToList(),
                ["Maşın"] = products.Where(p => p.Category == ProductCategory.Maşın).ToList(),
                ["Digər"] = products.Where(p => p.Category == ProductCategory.Digər).ToList()
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };

            File.WriteAllText(_path, JsonConvert.SerializeObject(grouped, settings));
        }

        public override List<Product> Deserialize()
        {
            if (!File.Exists(_path))
                return new List<Product>();

            string json = File.ReadAllText(_path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<Product>();

            if (json.TrimStart().StartsWith("["))
            {
                var list = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
                if (list.Count > 0) Product.SyncCounter(list.Max(p => p.Id));
                return list;
            }

            var grouped = JsonConvert.DeserializeObject<Dictionary<string, List<Product>>>(json);
            if (grouped == null) return new List<Product>();

            List<Product> all = grouped.Values
                .Where(l => l != null)
                .SelectMany(l => l)
                .ToList();

            if (all.Count > 0)
                Product.SyncCounter(all.Max(p => p.Id));

            return all;
        }
    }
}