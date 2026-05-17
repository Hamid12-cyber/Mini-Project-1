using Mini_Project_1.Models;
using Mini_Project_1.Repostories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Services
{
    internal class ProductServices
    {
        internal ProductRepostory ProductRepo { get; set; } = new();
        public object OrderRepo { get; internal set; }

        public void CreateProduct(string name, decimal price, int stock)
        {
            List<Product> products = ProductRepo.Deserialize();
            foreach (Product p in products)
            {
                if (p.Name.ToLower () == name.Trim().ToLower() )
                {
                    throw new ArgumentException($"{name} adlı məhsul artıq müvcuddur.Birdaha sınayın"); 
                }
            }
            if (price <= 0 || stock < 0)
            {
                Console.WriteLine($"Qiymət 0-dan böyük, stok mənfi olmamalıdır!");
                return;
            }
            Product newProduct = new Product(name, price, stock);
            products.Add(newProduct);
            ProductRepo.Serialize(products);
            Console.WriteLine("Məhsul uğurla yaradıldı və fayla yazıldı.");
        }
        public void DeleteProduct(int id)
        {
           List<Product> products = ProductRepo.Deserialize();     
           
            Product productToDelete = null;
            
            foreach (var p in products)
            {
                if (p.Id == id)
                {
                   productToDelete = p;
                    break;
                }
            }
                        
            if (productToDelete != null)
            {
                products.Remove(productToDelete); 
                ProductRepo.Serialize(products);              
                Console.WriteLine($" {id} nömrəli məhsul silindi.");
            }
            else
            {
                Console.WriteLine($"Bu ID-li məhsul tapılmadı!");
            }
        }
        public void GetProductById(int id)
        {           
            List<Product> products = ProductRepo.Deserialize();
                        
            Product? foundProduct = products.Find(p => p.Id == id);
            
            if (foundProduct != null)
            {
                Console.WriteLine("\n--- Məhsul Tapıldı ---");
                foundProduct.PrintInfo(); 
            }
            else
            {
                Console.WriteLine($"{id} ID-li məhsul bazada yoxdur!");
            }
        }
        public void ShowAllProducts() 
        {
            List <Product> products = ProductRepo.Deserialize();
            if (products.Count == 0) 
            {
                Console.WriteLine($"Hazırda məhsul yoxdur");
                return;
            }
            Console.WriteLine($"---Bütün məhsullar---");
            foreach (var p in products) 
            {
                string stockStatus = p.Stock == 0 ? " - [Out of Stock]" : "";
                p.PrintInfo();
            }

        }
        public void RefillProduct(int id, int amount) 
        {
            if (amount <= 0) 
            {
                Console.WriteLine($"Daxil olunan stock 0 vəya mənfi ola bilməz");
            }
            List<Product>products = ProductRepo.Deserialize();
            
            Product? productToUpdate = products.Find(p=>p.Id == id);

            if (productToUpdate == null) 
            {
                Console.WriteLine($"Məhsul tapılmadı");
                return;
            }
            productToUpdate.Stock += amount;
            ProductRepo.Serialize(products);
            Console.WriteLine($"Uğurlu! '{productToUpdate.Name}' məhsulunun yeni stoku: {productToUpdate.Stock}");

        }
    }












    
    
}
