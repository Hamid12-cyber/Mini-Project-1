using System.Threading;
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ── YENİ SİFARİŞ ──");
            Console.ResetColor();

            Write("\n  Email: ", ConsoleColor.Yellow);
            string email = Console.ReadLine();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Print("  Email formatı düzgün deyil, @ mütləq olmalıdır.", ConsoleColor.Red);
                return;
            }

            List<OrderItem> orderitems = new List<OrderItem>();
            List<Product> allProducts = productServices.ProductRepo.Deserialize();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n  +================================+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  |       ENDİRİM SHERTLERİ       |");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================+");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  | 100 AZN ve daha cox  ->  5%   |");
            Console.WriteLine("  | 300 AZN ve daha cox  ->  10%  |");
            Console.WriteLine("  | (Catdirilma haqqina aid deyil)|");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================+");
            Console.ResetColor();

            Print("\n  Movcud mehsullar:", ConsoleColor.Yellow);
            Product.PrintHeader();
            foreach (var pr in allProducts)
            {
                if (pr.Stock <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("  | {0,-5} | {1,-23} | {2,-13} | {3,-9} |",
                        pr.Id, pr.Name, pr.Price.ToString("0.00") + " AZN", "OUT STOCK");
                    Console.ResetColor();
                }
                else pr.PrintInfo();
            }
            Product.PrintFooter();

            while (true)
            {
                Write("\n  Mehsulun ID-sini daxil edin (cixmaq ucun 0): ", ConsoleColor.Yellow);

                if (!int.TryParse(Console.ReadLine(), out int productId) || productId < 0)
                { Print("  Duzgun reqem daxil edin.", ConsoleColor.Red); continue; }

                Console.Clear();
                if (productId == 0) break;

                Product product = allProducts.Find(p => p.Id == productId);
                if (product == null) { Print("  Mehsul tapilmadi!", ConsoleColor.Red); continue; }
                if (product.Stock <= 0) { Print($"  '{product.Name}' stokda yoxdur!", ConsoleColor.Red); continue; }

                Print($"  {product.Name} | Stok: {product.Stock} | Qiymet: {product.Price} AZN", ConsoleColor.White);
                Write("  Nece eded almaq isteyirsiniz: ", ConsoleColor.Yellow);

                if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                { Print("  Düzgün miqdar daxil edin.", ConsoleColor.Red); continue; }

                if (count > product.Stock)
                { Print($"  Yalnız {product.Stock} ədəd mövcuddur.", ConsoleColor.Red); continue; }

                OrderItem existing = orderitems.Find(o => o.Product.Id == product.Id);
                if (existing != null)
                {
                    existing.Count += count;
                    Print($"  '{product.Name}' x{count} elave edildi. Umumi: x{existing.Count}", ConsoleColor.Yellow);
                }
                else
                {
                    orderitems.Add(new OrderItem(product, count));
                    Print($"  '{product.Name}' x{count} sebete elave edildi.", ConsoleColor.Yellow);
                }

                product.Stock -= count;
                productServices.ProductRepo.Serialize(allProducts);

                Write("  Baska mehsul elave etmek isteyirsiniz? (b / x): ", ConsoleColor.Yellow);
                if (Console.ReadLine()?.Trim().ToLower() != "b") break;
            }

            Console.Clear();

            if (orderitems.Count == 0)
            { Print("  Heç məhsul seçilmədi, sifariş ləğv edildi.", ConsoleColor.Yellow); return; }

            decimal deliveryFee = 0;
            string deliveryType = "";

            while (true)
            {
                Print("\n  Catdirilma növünü seçin:", ConsoleColor.Yellow);
                Print("  1 - Özüm götürəcəm (Pulsuz)", ConsoleColor.White);
                Print("  2 - Kuryer ilə (5 AZN)", ConsoleColor.White);
                Print("  0 - Sifarişi ləğv et", ConsoleColor.DarkRed);
                Write("  Seciminiz: ", ConsoleColor.Yellow);

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
                    string address = "";
                    while (true)
                    {
                        Console.Clear();
                        Write("  Catdirilma unvanini daxil edin (geri ucun 0): ", ConsoleColor.Yellow);
                        address = Console.ReadLine()?.Trim();
                        if (address == "0") { Print("  Catdirilma secimie geri qayidildi.", ConsoleColor.Yellow); break; }
                        if (string.IsNullOrWhiteSpace(address))
                        { Print("  Unvan bos ola bilmez, yeniden daxil edin.", ConsoleColor.Red); Thread.Sleep(800); continue; }
                        deliveryType = "Kuryer ile: " + address;
                        deliveryFee = 5;
                        goto deliveryDone;
                    }
                    continue;
                }
                else Print("  Yanlış seçim.", ConsoleColor.Red);
            }

        deliveryDone:
            List<Order> orders = OrderRepo.Deserialize();
            Order order = new Order(email, orderitems, deliveryType, deliveryFee);
            orders.Add(order);
            OrderRepo.Serialize(orders);

            order.PrintReceipt();

            Print("\n  İzləmə kodlarınız:", ConsoleColor.Yellow);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  ┌──────────────────────────────────────┬─────────────────────────┐");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  │ İzləmə Kodu                          │ Məhsul                  │");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  ├──────────────────────────────────────┼─────────────────────────┤");
            Console.ResetColor();
            foreach (var item in order.Items)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  │ {0,-36} │ {1,-23} │", item.Id, item.Product.Name);
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  └──────────────────────────────────────┴─────────────────────────┘");
            Console.ResetColor();

            if (order.DiscountPercent > 0)
                Print($"\n  {order.DiscountPercent}% endirim qazandınız! Qənaət: {order.Discount} AZN", ConsoleColor.Yellow);
        }

        public void TrackOrder()
        {
            Write("\n  İzləmə kodunu daxil edin: ", ConsoleColor.Yellow);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (!Guid.TryParse(input, out Guid trackingId))
            { Print("  Düzgün izləmə kodu daxil edin.", ConsoleColor.Red); return; }

            List<Order> orders = OrderRepo.Deserialize();

            Order foundOrder = null;
            OrderItem foundItem = null;

            foreach (var o in orders)
            {
                foreach (var item in o.Items)
                {
                    if (item.Id == trackingId)
                    { foundOrder = o; foundItem = item; break; }
                }
                if (foundOrder != null) break;
            }

            if (foundOrder == null)
            { Print("  Bu izləmə koduna uyğun sifariş tapılmadı!", ConsoleColor.Red); return; }

            string statusMessage = foundOrder.Status switch
            {
                OrderStatus.Pending => "Sifarişiniz hazirlanir, tezliklə yola salinacaq .",
                OrderStatus.Confirmed => "Sifarişiniz təsdiqləndi, catdirilmaya hazirlanir.",
                OrderStatus.Completed => "Sifarişiniz ugurla catdirildi.",
                OrderStatus.Cancelled => "Sifarişiniz ləğv edilmişdir.",
                _ => "Məlumat yoxdur."
            };

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  |              *** İZLƏMƏ ***                    |");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Sifariş No    : ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"#{foundOrder.Id,-32}|");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Məhsul        : ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"{foundItem.Product.Name,-32}|");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Ədəd          : ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"{foundItem.Count,-32}|");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Məbləğ        : ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"{foundItem.SubTotal:0.00} AZN{new string(' ', 27)}|");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Tarix         : ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"{foundOrder.OrderedAt:dd.MM.yyyy HH:mm}{new string(' ', 18)}|");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  | Status        : ");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"{foundOrder.Status,-32}|");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +------------------------------------------------+");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  | {statusMessage,-48}|");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ResetColor();
        }

        public void ShowAllOrders()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ══ BÜTÜN SİFARİŞLƏR ══");
            Console.ResetColor();

            List<Order> orders = OrderRepo.Deserialize();

            if (orders == null || orders.Count == 0)
            {
                Print("\n  Sistemdə heç bir sifariş tapılmadı.", ConsoleColor.Yellow);
                return;
            }

            const int startX = 2;  
            const int boxWidth = 56; 
            int endX = startX + boxWidth - 1;

            foreach (var order in orders)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n  ╔{new string('═', boxWidth - 2)}╗");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  ║ ");
                Console.Write($"Sifariş #{order.Id} │ {order.OrderedAt:dd.MM.yyyy HH:mm} │ Status: {order.Status}");
                DrawRightBorder(endX);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  ╠{new string('═', boxWidth - 2)}╣");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  ║ ");
                Console.Write("{0,-5} {1,-20} {2,-8} {3,-15}", "ID", "Məhsul", "Ədəd", "Məbləğ");
                DrawRightBorder(endX);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  ╠{new string('═', boxWidth - 2)}╣");
                Console.ResetColor();

                foreach (var item in order.Items)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  ║ ");
                    string priceStr = item.SubTotal.ToString("0.00") + " AZN";

                    string productName = item.Product.Name.Length > 20 ? item.Product.Name.Substring(0, 17) + "..." : item.Product.Name;

                    Console.Write("{0,-5} {1,-20} {2,-8} {3,-15}",
                        item.Product.Id,
                        productName,
                        item.Count,
                        priceStr);

                    DrawRightBorder(endX);
                    Console.ResetColor();
                }
                               
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  ╠{new string('═', boxWidth - 2)}╣");

                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("  ║ "); Console.Write($"Email      : {order.Email}"); DrawRightBorder(endX);
                Console.Write("  ║ "); Console.Write($"Çatdırılma : {order.DeliveryType}"); DrawRightBorder(endX);

                if (order.DiscountPercent > 0)
                {
                    Console.Write("  ║ "); Console.Write($"Endirim    : %{order.DiscountPercent}"); DrawRightBorder(endX);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  ║ ");
                Console.Write($"Yekun      : {order.FinalTotal:0.00} AZN");
                DrawRightBorder(endX);
                
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  ╚{new string('═', boxWidth - 2)}╝");
                Console.ResetColor();
            }
        }

        private void DrawRightBorder(int targetColumn)
        {
            
            Console.CursorLeft = targetColumn;            
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("║");
            Console.ForegroundColor = oldColor;
        }

        public void ChangeOrderStatus()
        {
            List<Order> orders = OrderRepo.Deserialize();
            if (orders == null || orders.Count == 0)
            {
                Print("\n  Sistemdə heç bir sifariş yoxdur.", ConsoleColor.Yellow);
                return;
            }

            Write("  Statusunu dəyişmək istədiyiniz Sifariş ID (Geri qayıtmaq üçün '0' yazın): ", ConsoleColor.Yellow);
            string inputId = Console.ReadLine()?.Trim();

            if (inputId == "0") return; 

            if (!int.TryParse(inputId, out int orderId))
            {
                Print("  Düzgün ID daxil edin.", ConsoleColor.Red);
                return;
            }

            Order order = orders.Find(o => o.Id == orderId);
            if (order == null)
            {
                Print("  Bu ID-li sifariş tapılmadı!", ConsoleColor.Red);
                return;
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                Print("  Ləğv edilmiş sifarişin statusu dəyişdirilə bilməz.", ConsoleColor.Red);
                return;
            }

            Print($"\n  Cari Status: [{order.Status}]", ConsoleColor.Yellow);
            Print("  Yeni statusu seçin (Geri qayıtmaq üçün '0' yazın):", ConsoleColor.Yellow);
            Print("  1. Pending", ConsoleColor.White);
            Print("  2. Confirmed", ConsoleColor.White);
            Print("  3. Completed", ConsoleColor.White);
            Write("  Seçiminiz (1-3): ", ConsoleColor.Yellow);

            string choice = Console.ReadLine()?.Trim();
            if (choice == "0") return; 

            switch (choice)
            {
                case "1": order.Status = OrderStatus.Pending; break;
                case "2": order.Status = OrderStatus.Confirmed; break;
                case "3": order.Status = OrderStatus.Completed; break;
                default:
                    Print("  Yanlış seçim. Status dəyişdirilmədi.", ConsoleColor.Red);
                    return;
            }

            OrderRepo.Serialize(orders);
            Print($"\n  #{orderId} sifarişin statusu '{order.Status}' olaraq yeniləndi.", ConsoleColor.Yellow);

            Print("\n  [ Enter - menyuya qayıt ]", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        public void ShowOrdersByEmail()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ══ MÜŞTƏRİ TARİXÇƏSİ ══");
            Console.ResetColor();

            Write("  Email daxil edin (Geri qayıtmaq üçün '0' yazın): ", ConsoleColor.Yellow);
            string email = Console.ReadLine()?.Trim().ToLower();

            if (email == "0") return; 

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Print("  Düzgün email daxil edin.", ConsoleColor.Red);
                return;
            }

            List<Order> orders = OrderRepo.Deserialize();
            if (orders == null) orders = new List<Order>();

            List<Order> userOrders = orders
                .Where(o => o.Email.ToLower() == email)
                .OrderByDescending(o => o.OrderedAt)
                .ToList();

            if (userOrders.Count == 0)
            {
                Print($"\n  '{email}' emaili ilə heç bir sifariş tapılmadı.", ConsoleColor.Yellow);
                Print("\n  [ Enter - menyuya qayıt ]", ConsoleColor.DarkGray);
                Console.ReadLine();
                return;
            }

            Print($"\n  {email} - SİFARİŞ TARİXÇƏSİ", ConsoleColor.Yellow);
            Print($"  Ümumi sifariş sayı: {userOrders.Count}\n", ConsoleColor.White);
            const int startX = 2;
            const int boxWidth = 60;
            int endX = startX + boxWidth - 1;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  ┌{new string('─', boxWidth - 2)}┐");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  │ ");
            Console.Write("{0,-6} │ {1,-14} │ {2,-18} │ {3,-12}", "ID", "Status", "Tarix", "Məbləğ");
            Console.CursorLeft = endX; Console.WriteLine("│");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  ├{new string('─', boxWidth - 2)}┤");
            Console.ResetColor();

            foreach (var o in userOrders)
            {
                string disc = o.DiscountPercent > 0 ? "*" : " ";
                string priceStr = o.FinalTotal.ToString("0.00") + " AZN" + disc;

                Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write("  │ ");
                Console.ForegroundColor = ConsoleColor.White; Console.Write($"#{o.Id,-4} ");
                Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write($"{o.Status,-14}");
                Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.White; Console.Write($"{o.OrderedAt:dd.MM.yyyy HH:mm} ");
                Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write("│ ");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write($"{priceStr,-12}");

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.CursorLeft = endX;
                Console.WriteLine("│");
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  └{new string('─', boxWidth - 2)}┘");
            Console.ResetColor();

            decimal totalSpent = userOrders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .Sum(o => o.FinalTotal);

            Print($"\n  Ümumi xərclənən: {totalSpent:0.00} AZN", ConsoleColor.Yellow);
            Print("\n  [ Enter - menyuya qayıt ]", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        public void CancelOrder(ProductServices productServices)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  ── SİFARİŞİ LƏĞV ET ──");
            Console.ResetColor();

            List<Order> orders = OrderRepo.Deserialize();

            if (orders.Count == 0)
            { Print("\n  Sistemdə heç bir sifariş yoxdur.", ConsoleColor.Yellow); return; }

            Write("  Ləğv etmək istədiyiniz Sifariş ID: ", ConsoleColor.Yellow);
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            { Print("  Düzgün ID daxil edin.", ConsoleColor.Red); return; }

            Order order = orders.Find(o => o.Id == orderId);
            if (order == null) { Print("  Bu ID-li sifariş tapılmadı!", ConsoleColor.Red); return; }

            if (order.Status != OrderStatus.Pending)
            {
                Print($"\n  Yalnız 'Pending' sifarişlər ləğv oluna bilər.", ConsoleColor.Red);
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
                    Print($"  '{product.Name}' stoka geri qaytarıldı (+{item.Count})", ConsoleColor.Yellow);
                }
            }
            productServices.ProductRepo.Serialize(allProducts);

            order.Status = OrderStatus.Cancelled;
            OrderRepo.Serialize(orders);
            Print($"\n  #{orderId} nomreli sifariş ləğv edildi.", ConsoleColor.Yellow);
        }
    }
}
