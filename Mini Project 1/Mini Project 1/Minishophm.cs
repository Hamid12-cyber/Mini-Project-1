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
            "10. Track Order",
            "11. Cancel Order",
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
                        productServices.CreateProduct();
                        break;

                    case "2":
                        Console.Clear();
                        ConsoleAnimation.Loading("Silmə paneli", 900);
                        productServices.DeleteProduct();
                        break;

                    case "3":
                        Console.Clear();
                        ConsoleAnimation.Loading("Axtarılır", 400);
                        productServices.GetProductById();
                        break;

                    case "4":
                        Console.Clear();
                        ConsoleAnimation.Loading("Məhsullar yüklənir", 900);
                        productServices.ShowAllProducts();
                        break;

                    case "5":
                        Console.Clear();
                        ConsoleAnimation.Loading("Stok paneli", 900);
                        productServices.RefillProduct();
                        break;

                    case "6":
                        Console.Clear();
                        ConsoleAnimation.Loading("Sifariş paneli açılır", 900);
                        orderProduct.Orderproduct(productServices);
                        break;

                    case "7":
                        Console.Clear();
                        ConsoleAnimation.Loading("Sifarişlər yüklənir", 900);
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
                        orderProduct.ShowOrdersByEmail();
                        break;

                    case "10":
                        Console.Clear();
                        ConsoleAnimation.Loading("İzləmə paneli", 900);
                        orderProduct.TrackOrder();
                        break;
                                            
                    case "11":
                        Console.Clear();
                        ConsoleAnimation.Loading("Ləğvetmə paneli", 900);
                        orderProduct.CancelOrder(productServices);
                        break;

                    case "0":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n  Sistem bağlanır...");
                        Console.ResetColor();
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
                    Console.Write("\n  [ Enter - menyuya qayıt ]");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }
        }
    }
}
