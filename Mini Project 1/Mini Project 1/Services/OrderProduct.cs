using Mini_Project_1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Services
{
    internal class OrderProduct
    {
        private static readonly string _path = @"C:\Users\hamidim\Mini-Project-1\Mini Project 1\Mini Project 1\Data\Orders.json";
        protected void Serialize(List<Order> items)
        {
            string json = JsonConvert.SerializeObject(items);
            using (StreamWriter sw = new StreamWriter(_path)) 
            {
                sw.Write(json);
            }
        }
        protected List<Order> Deserialize() 
        {
            string json;
            using (StreamReader sr = new(_path)) 
            {
                json = sr.ReadToEnd();
            }
            List <Order> list = JsonConvert.DeserializeObject <List<Order>>(_path);
            if (list.Count > 0) Order.SyncCounter(list.Max(p => p.Id));
            return list ?? new List<Order>();
        }
        public void Orderproduct(ProductServices productServices) 
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Console.WriteLine($"Email formatı düzgün deyil (@ mütləq olmalıdır");
                return;
            }
            List <OrderItem> orderitems = new List<OrderItem>();
            Console.Write("Almaq istədiyiniz məhsulun ID-sini daxil edin: ");
            int productId = int.Parse(Console.ReadLine());

            if (productId == 0 || productId <0 )
            {
                Console.WriteLine($"Düzgün rəqəm daxil edin.");
                return;
            }
            string json = File.ReadAllText("products.json");
            List<Product> products = JsonSerializer.Deserialize<List<Product>>(json);
            Product product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                Console.WriteLine("Məhsul tapılmadı!");
                return;
            }

        }
    }
}
