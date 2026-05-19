using Mini_Project_1.Animations;
using Mini_Project_1.Services;

namespace Mini_Project_1
{
    internal class Minishophm
    {
        private static readonly string[] MenuItems =
        {
            "1.  Create Product",
            "2.  Delete Product",
            "3.  Get Product By Id",
            "4.  Show All Products",
            "5.  Refill Product",
            "6.  Order Product",
            "7.  Show All Orders",
            "8.  Change Order Status",
            "9.  Show Orders By Email",
            "10. Cancel Order",
            "0.  Exit"
        };

        public void Run()
        {
            ConsoleAnimation.SplashScreen("MINI SHOP");

            ProductServices productServices = new ProductServices();
            OrderProduct orderProduct = new OrderProduct();

            bool isRunning = true;

            while (isRunning)
            {
                ConsoleAnimation.PrintMenu("MINI SHOP", MenuItems);

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        ConsoleAnimation.Loading("Məhsul yaratma paneli", 900);
                        Console.Write("  Ad: "); string name = Console.ReadLine();
                        Console.Write("  Qiymət: "); decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("  Stok: "); int stock = int.Parse(Console.ReadLine());
                        productServices.CreateProduct(name, price, stock);
                        break;

                    case "2":
                        Console.Clear();
                        ConsoleAnimation.Loading("Silmə paneli", 900);
                        Console.Write("  Silinəcək ID: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                            productServices.DeleteProduct(deleteId);
                        else
                            ConsoleAnimation.Error("Düzgün ID daxil edin.");
                        break;

                    case "3":
                        Console.Clear();
                        ConsoleAnimation.Loading("Axtarılır", 900);
                        Console.Write("  Axtarılacaq ID: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId))
                            productServices.GetProductById(searchId);
                        else
                            ConsoleAnimation.Error("Düzgün ID daxil edin.");
                        break;

                    case "4":
                        Console.Clear();
                        ConsoleAnimation.Loading("Məhsullar yüklənir", 900);
                        productServices.ShowAllProducts();
                        break;

                    case "5":
                        Console.Clear();
                        ConsoleAnimation.Loading("Stok paneli", 900);
                        Console.Write("  Məhsulun ID-sini daxil edin: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        { ConsoleAnimation.Error("ID düzgün deyil."); break; }
                        Console.Write("  Artırılacaq miqdar: ");
                        if (!int.TryParse(Console.ReadLine(), out int amount))
                        { ConsoleAnimation.Error("Miqdar düzgün deyil."); break; }
                        productServices.RefillProduct(id, amount);
                        break;

                    case "6":
                        Console.Clear();
                        ConsoleAnimation.Loading("Sifariş paneli açılır", 900);
                        ConsoleAnimation.TypeWriteLine("\n  ── YENİ SİFARİŞ ──", delayMs: 14);
                        orderProduct.Orderproduct(productServices);
                        break;

                    case "7":
                        Console.Clear();
                        ConsoleAnimation.Loading("Sifarişlər yüklənir", 900);
                        ConsoleAnimation.TypeWriteLine("\n  ══ BÜTÜN SİFARİŞLƏR ══", delayMs: 12);
                        orderProduct.ShowAllOrders();
                        break;

                    case "8":
                        Console.Clear();
                        ConsoleAnimation.Loading("Status paneli", 900);
                        orderProduct.ChangeOrderStatus();
                        break;

                    case "9":
                        Console.Clear();
                        ConsoleAnimation.Loading("Tarixçə yüklənir", 900);
                        ConsoleAnimation.TypeWriteLine("\n  ── MÜŞTƏRİ TARİXÇƏSİ ──", delayMs: 12);
                        orderProduct.ShowOrdersByEmail();
                        break;

                    case "10":
                        Console.Clear();
                        ConsoleAnimation.Loading("Ləğvetmə paneli", 900);
                        ConsoleAnimation.TypeWriteLine("\n  ── SİFARİŞİ LƏĞV ET ──", delayMs: 12);
                        orderProduct.CancelOrder(productServices);
                        break;

                    case "0":
                        Console.Clear();
                        ConsoleAnimation.TypeWriteLine("\n  Sistem bağlanır...", delayMs: 30);
                        ConsoleAnimation.Loading("Çıxılır", 700);
                        isRunning = false;
                        break;

                    default:
                        ConsoleAnimation.Warning("Yanlış seçim! Yenidən cəhd edin.");
                        break;
                }

                if (isRunning && choice != "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("\n  [ Enter — menyuya qayıt ]");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }
        }
    }
}
