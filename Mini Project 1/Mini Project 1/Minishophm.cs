using Mini_Project_1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1
{
    internal class Minishophm
    {
        public void Run() 
        {
            ProductServices productServices = new ProductServices();
            OrderProduct orderProduct = new OrderProduct();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.Write("Secim edin:");
                Console.WriteLine($"\n1. Create Product\n2. Delete Product\n3. Get Product By Id\n4. Show All Product\n5. Refill Product\n6. Order Product\n7. Show All Orders\n8.Change Order Status");
                Console.WriteLine("0. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Ad: "); string name = Console.ReadLine();
                        Console.Write("Qiymet: "); decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("Stok: "); int stock = int.Parse(Console.ReadLine());
                        productServices .CreateProduct(name, price, stock);
                        break;

                    case "2":
                        Console.Clear();
                        Console.Write("Silinecek ID: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            productServices.DeleteProduct(deleteId);
                        }
                        else
                        {
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        }
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Axtarilacaq ID: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId))
                        {
                            productServices.GetProductById(searchId);
                        }
                        else
                        {
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        }
                        break;
                    case "4":
                        Console.Clear(); 
                        productServices.ShowAllProducts();
                        break;
                    case "5":
                        Console.Clear();
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
                        Console.Clear();
                        Console.WriteLine("\n--- Yeni Sifariş ---");
                        orderProduct.Orderproduct(productServices);
                        break;
                    case "7":
                        Console.Clear();
                        Console.WriteLine("\n================ BÜTÜN SİFARİŞLƏR ================");
                        orderProduct.ShowAllOrders();
                        break;
                    case "8":
                        Console.Clear();
                        orderProduct.ChangeOrderStatus();
                        break;
                    case "0":
                        Console.Clear();
                        isRunning = false;
                        Console.WriteLine("Proqramdan cixildi.");
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Yanlis secim! Yeniden cehd edin.");
                        break;

                }
            }
        }

    }
}
