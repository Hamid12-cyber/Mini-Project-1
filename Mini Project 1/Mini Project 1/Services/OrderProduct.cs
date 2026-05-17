using Mini_Project_1.Enums;
using Mini_Project_1.Models;
using Mini_Project_1.Repostories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Services
{
    internal class OrderProduct
    {          
        internal OrdersRepostory OrderRepo { get; set; } = new();
        public void Orderproduct(ProductServices productServices)
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Console.WriteLine($"Email formatı düzgün deyil @ mütləq olmalıdır");
                return;
            }
            List<OrderItem> orderitems = new List<OrderItem>();

            List<Product> allProducts = productServices.ProductRepo.Deserialize();

            while (true)
            {
                Console.Write("\nAlmaq istədiyiniz məhsulun ID-sini daxil edin (çıxmaq üçün 0): ");

                if (!int.TryParse(Console.ReadLine(), out int productId) || productId < 0)
                {
                    Console.WriteLine("Düzgün rəqəm daxil edin.");
                    continue;
                }

                if (productId == 0)
                    break;

                Product product = allProducts.Find(p => p.Id == productId);

                if (product == null)
                {
                    Console.WriteLine("Məhsul tapılmadı!");
                    continue;
                }

                if (product.Stock <= 0)
                {
                    Console.WriteLine("Məhsul stokda yoxdur!");
                    continue;
                }

                Console.WriteLine($"  {product.Name} | Stok: {product.Stock} | Qiymət: {product.Price} Azn");
                Console.Write("Neçə ədəd almaq istəyirsiniz: ");

                if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                {
                    Console.WriteLine("Düzgün miqdar daxil edin.");
                    continue;
                }

                if (count > product.Stock)
                {
                    Console.WriteLine($"Yalnız {product.Stock} ədəd mövcuddur.");
                    continue;
                }

                orderitems.Add(new OrderItem(product, count));
                product.Stock -= count;
                productServices.ProductRepo.Serialize(allProducts);

                Console.WriteLine($"✓ '{product.Name}' x{count} səbətə əlavə edildi.");

                Console.Write("Başqa məhsul əlavə etmək istəyirsiniz? (b - Bəli / x - Xeyr): ");
                string choice = Console.ReadLine()?.Trim().ToLower();

                if (choice != "b" && choice != "beli")
                    break;
            }

            if (orderitems.Count == 0)
            {
                Console.WriteLine("Heç məhsul seçilmədi, sifariş ləğv edildi.");
                return;
            }                  
            List<Order> orders = OrderRepo.Deserialize();
            Order order = new Order(email, orderitems);
            orders.Add(order);
            OrderRepo.Serialize(orders);

            Console.WriteLine($"\n✓ Sifariş yaradıldı! Cəmi: {order.Total} Azn");
        }
        public void ShowAllOrders()
        {
            List<Order> orders = OrderRepo.Deserialize();

            if (orders.Count == 0)
            {
                Console.WriteLine("\nSistemdə heç bir sifariş tapılmadı.");
                return;
            }            
            foreach (var order in orders)
            {
                Console.WriteLine($"Sifariş ID: {order.Id}");
                Console.WriteLine($"Müştəri: {order.Email}");
                Console.WriteLine($"Tarix: {order.OrderedAt}");                           
                Console.WriteLine($"Status: [{order.Status}]");
                Console.WriteLine($"Ümumi Məbləğ: {order.Total} Azn");
                Console.WriteLine("Məhsullar:");

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"   - {item.Product.Name} | {item.Count} ədəd x {item.Price} Azn");
                }                
            }
        }
        public void ChangeOrderStatus()
        {           
            List<Order> orders = OrderRepo.Deserialize();
            if (orders.Count == 0)
            {
                Console.WriteLine("\nSistemdə heç bir sifariş yoxdur.");
                return;
            }
            
            Console.Write("Statusunu dəyişmək istədiyiniz Sifariş ID-sini daxil edin: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Düzgün ID formatı daxil edin (rəqəm olmalıdır).");
                return;
            }            
            Order order = orders.Find(o => o.Id == orderId);
            if (order == null)
            {
                Console.WriteLine("Bu ID-li sifariş tapılmadı!");
                return;
            }                       
            Console.WriteLine($"\nSifariş tapıldı! Cari Status: [{order.Status}]");
            Console.WriteLine("Zəhmət olmasa yeni statusu seçin:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Confirmed");
            Console.WriteLine("3. Completed");
            Console.Write("Seçiminiz (1-3): ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    order.Status = OrderStatus.Pending;
                    break;
                case "2":
                    order.Status = OrderStatus.Confirmed;
                    break;
                case "3":
                    order.Status = OrderStatus.Completed;
                    break;
                default:
                    Console.WriteLine("Yanlış seçim etdiniz! Sifarişin statusu dəyişdirilmədi.");
                    return;
            }
            OrderRepo.Serialize(orders);
            Console.WriteLine($"\n✓ {orderId} nömrəli sifarişin statusu uğurla '{order.Status}' olaraq yeniləndi.");
        }
    
    }
}
