using Mini_Project_1.Animations;
using Mini_Project_1.Enums;
using Mini_Project_1.Models;
using Mini_Project_1.Repostories;

namespace Mini_Project_1.Services
{
    internal class OrderProduct
    {
        internal OrdersRepostory OrderRepo { get; set; } = new();

        private static void Print(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public void Orderproduct(ProductServices productServices)
        {
            Write("\n  Email: ", ConsoleColor.Yellow);
            string email = Console.ReadLine();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Print("  ✗ Email formatı düzgün deyil, @ mütləq olmalıdır.", ConsoleColor.Red);
                return;
            }

            List<OrderItem> orderitems = new List<OrderItem>();
            List<Product> allProducts = productServices.ProductRepo.Deserialize();

            while (true)
            {
                Write("\n  Məhsulun ID-sini daxil edin (çıxmaq üçün 0): ", ConsoleColor.Yellow);

                if (!int.TryParse(Console.ReadLine(), out int productId) || productId < 0)
                {
                    Print("  ✗ Düzgün rəqəm daxil edin.", ConsoleColor.Red);
                    continue;
                }

                if (productId == 0) break;

                Product product = allProducts.Find(p => p.Id == productId);
                if (product == null)
                {
                    Print("  ✗ Məhsul tapılmadı!", ConsoleColor.Red);
                    continue;
                }

                Print($"  {product.Name} | Stok: {product.Stock} | Qiymət: {product.Price} AZN", ConsoleColor.White);
                Write("  Neçə ədəd almaq istəyirsiniz: ", ConsoleColor.Yellow);

                if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                {
                    Print("  ✗ Düzgün miqdar daxil edin.", ConsoleColor.Red);
                    continue;
                }

                if (count > product.Stock)
                {
                    Print($"  ✗ Yalnız {product.Stock} ədəd mövcuddur.", ConsoleColor.Red);
                    continue;
                }

                orderitems.Add(new OrderItem(product, count));
                product.Stock -= count;
                productServices.ProductRepo.Serialize(allProducts);

                Print($"  ✓ '{product.Name}' x{count} səbətə əlavə edildi.", ConsoleColor.Yellow);
                Write("  Başqa məhsul əlavə etmək istəyirsiniz? (b / x): ", ConsoleColor.Yellow);
                string ch = Console.ReadLine()?.Trim().ToLower();
                if (ch != "b" && ch != "bəli") break;
            }

            Console.Clear();

            if (orderitems.Count == 0)
            {
                Print("  Heç məhsul seçilmədi, sifariş ləğv edildi.", ConsoleColor.Yellow);
                return;
            }

            decimal deliveryFee = 0;
            string deliveryType = "";

            while (true)
            {
                Print("\n  Çatdırılma növünü seçin:", ConsoleColor.Yellow);
                Print("  1 - Özüm götürəcəm (Pulsuz)", ConsoleColor.White);
                Print("  2 - Kuryer ilə (5 AZN)", ConsoleColor.White);
                Print("  0 - Sifarişi ləğv et", ConsoleColor.DarkRed);
                Write("  Seçiminiz: ", ConsoleColor.Yellow);

                string deliveryChoice = Console.ReadLine()?.Trim();

                if (deliveryChoice == "0")
                {
                    Print("  Sifariş ləğv edildi.", ConsoleColor.Red);
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
                    Write("  Çatdırılma ünvanını daxil edin: ", ConsoleColor.Yellow);
                    string address = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        Print("  ✗ Ünvan boş ola bilməz.", ConsoleColor.Red);
                        continue;
                    }
                    deliveryType = "Kuryer ilə";
                    deliveryFee = 5;
                    break;
                }
                else
                {
                    Print("  ✗ Yanlış seçim.", ConsoleColor.Red);
                }
            }

            List<Order> orders = OrderRepo.Deserialize();
            Order order = new Order(email, orderitems, deliveryType, deliveryFee);
            orders.Add(order);
            OrderRepo.Serialize(orders);

            order.PrintReceipt();

            if (order.DiscountPercent > 0)
                Print($"\n  🎉 {order.DiscountPercent}% endirim qazandınız! Qənaət: {order.Discount} AZN", ConsoleColor.Yellow);
        }

        public void ShowOrdersByEmail()
        {
            Write("  Email daxil edin: ", ConsoleColor.Yellow);
            string email = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Print("  ✗ Düzgün email daxil edin.", ConsoleColor.Red);
                return;
            }

            List<Order> orders = OrderRepo.Deserialize();
            List<Order> userOrders = orders
                .Where(o => o.Email.ToLower() == email)
                .OrderByDescending(o => o.OrderedAt)
                .ToList();

            if (userOrders.Count == 0)
            {
                Print($"\n  '{email}' emaili ilə heç bir sifariş tapılmadı.", ConsoleColor.Yellow);
                return;
            }

            Print($"\n  ═══ {email} — SİFARİŞ TARİXÇƏSİ ═══", ConsoleColor.DarkRed);
            Print($"  Ümumi sifariş sayı: {userOrders.Count}\n", ConsoleColor.Yellow);

            foreach (var o in userOrders)
            {
                string disc = o.DiscountPercent > 0 ? $" (Endirim: -{o.Discount} AZN)" : "";
                Print($"  #{o.Id}  [{o.Status}]  {o.OrderedAt:dd.MM.yyyy}  →  {o.FinalTotal} AZN{disc}", ConsoleColor.White);
            }

            decimal totalSpent = userOrders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .Sum(o => o.FinalTotal);

            Print($"\n  Ümumi xərclənən: {totalSpent} AZN", ConsoleColor.Yellow);
            Print("  ══════════════════════════════════════", ConsoleColor.DarkRed);
        }

        public void CancelOrder(ProductServices productServices)
        {
            List<Order> orders = OrderRepo.Deserialize();

            if (orders.Count == 0)
            {
                Print("\n  Sistemdə heç bir sifariş yoxdur.", ConsoleColor.Yellow);
                return;
            }

            Write("  Ləğv etmək istədiyiniz Sifariş ID: ", ConsoleColor.Yellow);
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Print("  ✗ Düzgün ID daxil edin.", ConsoleColor.Red);
                return;
            }

            Order order = orders.Find(o => o.Id == orderId);
            if (order == null)
            {
                Print("  ✗ Bu ID-li sifariş tapılmadı!", ConsoleColor.Red);
                return;
            }

            if (order.Status != OrderStatus.Pending)
            {
                Print($"\n  ✗ Yalnız 'Pending' sifarişlər ləğv oluna bilər.", ConsoleColor.Red);
                Print($"  Cari status: [{order.Status}]", ConsoleColor.Yellow);
                return;
            }

            List<Product> allProducts = productServices.ProductRepo.Deserialize();
            foreach (var item in order.Items)
            {
                Product product = allProducts.Find(p => p.Id == item.Product.Id);
                if (product != null)
                {
                    product.Stock += item.Count;
                    Print($"  ↩ '{product.Name}' stoka geri qaytarıldı (+{item.Count})", ConsoleColor.Yellow);
                }
            }
            productServices.ProductRepo.Serialize(allProducts);

            order.Status = OrderStatus.Cancelled;
            OrderRepo.Serialize(orders);
            Print($"\n  ✓ #{orderId} nömrəli sifariş ləğv edildi.", ConsoleColor.Yellow);
        }

        public void ShowAllOrders()
        {
            List<Order> orders = OrderRepo.Deserialize();

            if (orders.Count == 0)
            {
                Print("\n  Sistemdə heç bir sifariş tapılmadı.", ConsoleColor.Yellow);
                return;
            }

            foreach (var order in orders)
            {
                Print("\n  ──────────────────────────────────────", ConsoleColor.DarkRed);
                Print($"  Sifariş ID : #{order.Id}", ConsoleColor.Yellow);
                Print($"  Müştəri    : {order.Email}", ConsoleColor.White);
                Print($"  Tarix      : {order.OrderedAt:dd.MM.yyyy HH:mm}", ConsoleColor.White);
                Print($"  Status     : [{order.Status}]", ConsoleColor.Yellow);
                Print($"  Çatdırılma : {order.DeliveryType} ({order.DeliveryFee} AZN)", ConsoleColor.White);
                Print("  Məhsullar  :", ConsoleColor.Yellow);
                foreach (var item in order.Items)
                    Print($"     - {item.Product.Name} | {item.Count} ədəd x {item.Price} AZN = {item.SubTotal} AZN", ConsoleColor.White);

                decimal ps = order.Items.Sum(i => i.SubTotal);
                Print($"  Subtotal   : {ps} AZN", ConsoleColor.White);
                if (order.DiscountPercent > 0)
                    Print($"  Endirim    : -{order.Discount} AZN ({order.DiscountPercent}%)", ConsoleColor.Yellow);
                Print($"  Çatdırılma : {order.DeliveryFee} AZN", ConsoleColor.White);
                Print($"  YEKUN      : {order.FinalTotal} AZN", ConsoleColor.Yellow);
            }
            Print("  ──────────────────────────────────────", ConsoleColor.DarkRed);
        }

        public void ChangeOrderStatus()
        {
            List<Order> orders = OrderRepo.Deserialize();
            if (orders.Count == 0)
            {
                Print("\n  Sistemdə heç bir sifariş yoxdur.", ConsoleColor.Yellow);
                return;
            }

            Write("  Statusunu dəyişmək istədiyiniz Sifariş ID: ", ConsoleColor.Yellow);
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Print("  ✗ Düzgün ID daxil edin.", ConsoleColor.Red);
                return;
            }

            Order order = orders.Find(o => o.Id == orderId);
            if (order == null)
            {
                Print("  ✗ Bu ID-li sifariş tapılmadı!", ConsoleColor.Red);
                return;
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                Print("  ✗ Ləğv edilmiş sifarişin statusu dəyişdirilə bilməz.", ConsoleColor.Red);
                return;
            }

            Print($"\n  Cari Status: [{order.Status}]", ConsoleColor.Yellow);
            Print("  Yeni statusu seçin:", ConsoleColor.Yellow);
            Print("  1. Pending", ConsoleColor.White);
            Print("  2. Confirmed", ConsoleColor.White);
            Print("  3. Completed", ConsoleColor.White);
            Write("  Seçiminiz (1-3): ", ConsoleColor.Yellow);

            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": order.Status = OrderStatus.Pending; break;
                case "2": order.Status = OrderStatus.Confirmed; break;
                case "3": order.Status = OrderStatus.Completed; break;
                default:
                    Print("  ✗ Yanlış seçim. Status dəyişdirilmədi.", ConsoleColor.Red);
                    return;
            }

            OrderRepo.Serialize(orders);
            Print($"\n  ✓ #{orderId} sifarişin statusu '{order.Status}' olaraq yeniləndi.", ConsoleColor.Yellow);
        }
    }
}
