using Mini_Project_1.Enums;
using Mini_Project_1.Models;
using Mini_Project_1.Repostories;

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
                Console.WriteLine("Email formatı düzgün deyil, @ mütləq olmalıdır.");
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

                if (productId == 0) break;

                Product product = allProducts.Find(p => p.Id == productId);
                if (product == null)
                {
                    Console.WriteLine("Məhsul tapılmadı!");
                    continue;
                }

                Console.WriteLine($"  {product.Name} | Stok: {product.Stock} | Qiymət: {product.Price} AZN");
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
                if (choice != "b" && choice != "bəli") break;
            }

            Console.Clear();

            if (orderitems.Count == 0)
            {
                Console.WriteLine("Heç məhsul seçilmədi, sifariş ləğv edildi.");
                return;
            }

            decimal deliveryFee = 0;
            string deliveryType = "";
            string address = "";

            while (true)
            {
                Console.WriteLine("\nÇatdırılma növünü seçin:");
                Console.WriteLine("  1 - Özüm götürəcəm (Pulsuz)");
                Console.WriteLine("  2 - Kuryer ilə (5 AZN)");
                Console.WriteLine("  0 - Sifarişi ləğv et");
                Console.Write("Seçiminiz: ");

                string deliveryChoice = Console.ReadLine()?.Trim();

                if (deliveryChoice == "0")
                {
                    Console.WriteLine("Sifariş ləğv edildi.");
                  
                    foreach (var item in orderitems)
                    {
                        var p = allProducts.Find(x => x.Id == item.Product.Id);
                        if (p != null) p.Stock += item.Count;
                    }
                    productServices.ProductRepo.Serialize(allProducts);
                    return;
                }
                else if (deliveryChoice == "1")
                {
                    deliveryType = "Özüm götürəcəm";
                    deliveryFee = 0;
                    break;
                }
                else if (deliveryChoice == "2")
                {
                    Console.Clear();
                    Console.Write("Çatdırılma ünvanını daxil edin: ");
                    address = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        Console.WriteLine("Ünvan boş ola bilməz, yenidən daxil edin.");
                        continue;
                    }
                    deliveryType = "Kuryer ilə";
                    deliveryFee = 5;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Yanlış seçim, yenidən cəhd edin.");
                }
            }

            List<Order> orders = OrderRepo.Deserialize();
            Order order = new Order(email, orderitems, deliveryType, deliveryFee);
            orders.Add(order);
            OrderRepo.Serialize(orders);

            order.PrintReceipt();

           
            if (order.DiscountPercent > 0)
                Console.WriteLine($"🎉 {order.DiscountPercent}% endirim qazandınız! Qənaət: {order.Discount} AZN");
        }
        public void ShowOrdersByEmail()
        {
            Console.Write("Email daxil edin: ");
            string email = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Console.WriteLine("Düzgün email daxil edin.");
                return;
            }

            List<Order> orders = OrderRepo.Deserialize();
            List<Order> userOrders = orders
                .Where(o => o.Email.ToLower() == email)
                .OrderByDescending(o => o.OrderedAt)
                .ToList();

            if (userOrders.Count == 0)
            {
                Console.WriteLine($"\n'{email}' emaili ilə heç bir sifariş tapılmadı.");
                return;
            }

            Console.WriteLine($"\n========= {email} - SİFARİŞ TARİXÇƏSİ =========");
            Console.WriteLine($"Ümumi sifariş sayı: {userOrders.Count}\n");

            foreach (var o in userOrders)
            {
                string discountInfo = o.DiscountPercent > 0
                    ? $" (Endirim: -{o.Discount} AZN)"
                    : "";
                Console.WriteLine($"  #{o.Id}  [{o.Status}]  {o.OrderedAt:dd.MM.yyyy}  →  {o.FinalTotal} AZN{discountInfo}");
            }

            decimal totalSpent = userOrders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .Sum(o => o.FinalTotal);
            Console.WriteLine($"\n  Ümumi xərclənən: {totalSpent} AZN");
            Console.WriteLine("==============================================");
        }
        public void CancelOrder(ProductServices productServices)
        {
            List<Order> orders = OrderRepo.Deserialize();

            if (orders.Count == 0)
            {
                Console.WriteLine("\nSistemdə heç bir sifariş yoxdur.");
                return;
            }

            Console.Write("Ləğv etmək istədiyiniz Sifariş ID-sini daxil edin: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Düzgün ID daxil edin (rəqəm olmalıdır).");
                return;
            }

            Order order = orders.Find(o => o.Id == orderId);
            if (order == null)
            {
                Console.WriteLine("Bu ID-li sifariş tapılmadı!");
                return;
            }

            if (order.Status != OrderStatus.Pending)
            {
                Console.WriteLine($"\n❌ Bu sifariş ləğv edilə bilməz.");
                Console.WriteLine($"   Yalnız 'Pending' statusundakı sifarişlər ləğv oluna bilər.");
                Console.WriteLine($"   Cari status: [{order.Status}]");
                return;
            }

            List<Product> allProducts = productServices.ProductRepo.Deserialize();
            foreach (var item in order.Items)
            {
                Product product = allProducts.Find(p => p.Id == item.Product.Id);
                if (product != null)
                {
                    product.Stock += item.Count;
                    Console.WriteLine($"   ↩ '{product.Name}' stoka geri qaytarıldı (+{item.Count})");
                }
            }
            productServices.ProductRepo.Serialize(allProducts);

            order.Status = OrderStatus.Cancelled;
            OrderRepo.Serialize(orders);

            Console.WriteLine($"\n✓ #{orderId} nömrəli sifariş ləğv edildi. Məhsullar stoka qaytarıldı.");
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
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"Sifariş ID : #{order.Id}");
                Console.WriteLine($"Müştəri    : {order.Email}");
                Console.WriteLine($"Tarix      : {order.OrderedAt:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Status     : [{order.Status}]");
                Console.WriteLine($"Çatdırılma : {order.DeliveryType} ({order.DeliveryFee} AZN)");
                Console.WriteLine("Məhsullar  :");
                foreach (var item in order.Items)
                    Console.WriteLine($"   - {item.Product.Name} | {item.Count} ədəd x {item.Price} AZN = {item.SubTotal} AZN");

                decimal productSum = order.Items.Sum(i => i.SubTotal);
                Console.WriteLine($"Subtotal   : {productSum} AZN");
                if (order.DiscountPercent > 0)
                    Console.WriteLine($"Endirim    : -{order.Discount} AZN ({order.DiscountPercent}%)");
                Console.WriteLine($"Çatdırılma : {order.DeliveryFee} AZN");
                Console.WriteLine($"YEKUN      : {order.FinalTotal} AZN");
            }
            Console.WriteLine(new string('-', 40));
        }

        // ==========================================
        // Change Order Status (köhnə metod — eyni)
        // ==========================================
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

            if (order.Status == OrderStatus.Cancelled)
            {
                Console.WriteLine("❌ Ləğv edilmiş sifarişin statusu dəyişdirilə bilməz.");
                return;
            }

            Console.WriteLine($"\nSifariş tapıldı! Cari Status: [{order.Status}]");
            Console.WriteLine("Yeni statusu seçin:");
            Console.WriteLine("  1. Pending");
            Console.WriteLine("  2. Confirmed");
            Console.WriteLine("  3. Completed");
            Console.Write("Seçiminiz (1-3): ");

            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": order.Status = OrderStatus.Pending; break;
                case "2": order.Status = OrderStatus.Confirmed; break;
                case "3": order.Status = OrderStatus.Completed; break;
                default:
                    Console.WriteLine("Yanlış seçim. Status dəyişdirilmədi.");
                    return;
            }

            OrderRepo.Serialize(orders);
            Console.WriteLine($"\n✓ #{orderId} sifarişin statusu '{order.Status}' olaraq yeniləndi.");
        }
    }
}
