using Mini_Project_1.Models;
using Mini_Project_1.Services;

namespace Mini_Project_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductServices productServices = new ProductServices();
            OrderProduct orderProduct = new OrderProduct();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.Write("Secim edin:");
                Console.WriteLine($"1. Create Product\n2. Delete Product\n3. Get Product By Id\n4. Show All Product\n5. Refill Product\n6. Order Product\n7. Show All Orders\n8.Change Order Status");
                Console.WriteLine("0. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Ad: "); string name = Console.ReadLine();
                        Console.Write("Qiymet: "); decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("Stok: "); int stock = int.Parse(Console.ReadLine());
                        productServices.CreateProduct(name, price, stock);
                        break;

                    case "2":
                        Console.Write("Silinecek ID: ");
                        if(int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            productServices.DeleteProduct(deleteId);
                        }
                        else
                        {
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        }
                        break;
                    case "3":
                         Console.Write("Axtarilacaq ID: ");
                        if (int .TryParse(Console.ReadLine(), out int searchId))
                        {
                            productServices.GetProductById(searchId);
                        }
                        else
                        {
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        }
                        break;
                    case "4":
                        productServices.ShowAllProducts();
                        break;
                    case "5":
                        Console.WriteLine("\n--- Məhsulun Stokunu Artırın---");
                        Console.Write("Məhsulun ID-sini daxil edin: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("ID düzgün formatda deyil (rəqəm olmalıdır).");
                            break;
                        }
                        Console.Write("Artırılacaq miqdarı daxil edin: ");
                        if (!int.TryParse(Console.ReadLine(), out int amount))
                        {
                            Console.WriteLine("Miqdar düzgün formatda deyil (rəqəm olmalıdır).");
                            break;
                        }
                        productServices.RefillProduct(id, amount);
                        break;
                    case "6":
                        Console.WriteLine("\n--- Yeni Sifariş ---");
                        orderProduct.Orderproduct(productServices);
                        break;
                    case "7":
                        Console.WriteLine("\n================ BÜTÜN SİFARİŞLƏR ================");
                        orderProduct.ShowAllOrders();
                        break;
                    case "8":
                        orderProduct.ChangeOrderStatus();
                        break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Proqramdan cixildi.");
                        break;

                    default:
                        Console.WriteLine("Yanlis secim! Yeniden cehd edin.");
                        break;




                }
            }
        }
    }
}
