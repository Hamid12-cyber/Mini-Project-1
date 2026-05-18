using Mini_Project_1.Services;

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
                Console.WriteLine("\n========== MENU ==========");
                Console.WriteLine("  1. Create Product");
                Console.WriteLine("  2. Delete Product");
                Console.WriteLine("  3. Get Product By Id");
                Console.WriteLine("  4. Show All Products");
                Console.WriteLine("  5. Refill Product");
                Console.WriteLine("  6. Order Product");
                Console.WriteLine("  7. Show All Orders");
                Console.WriteLine("  8. Change Order Status");
                Console.WriteLine("  9. Show Orders By Email");
                Console.WriteLine("  10. Cancel Order");
                Console.WriteLine("  0. Exit");
                Console.WriteLine("===========================");
                Console.Write("Seçim edin: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Ad: "); string name = Console.ReadLine();
                        Console.Write("Qiymət: "); decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("Stok: "); int stock = int.Parse(Console.ReadLine());
                        productServices.CreateProduct(name, price, stock);
                        break;

                    case "2":
                        Console.Clear();
                        Console.Write("Silinəcək ID: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                            productServices.DeleteProduct(deleteId);
                        else
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        break;

                    case "3":
                        Console.Clear();
                        Console.Write("Axtarılacaq ID: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId))
                            productServices.GetProductById(searchId);
                        else
                            Console.WriteLine("Zəhmət olmasa düzgün ID daxil edin.");
                        break;

                    case "4":
                        Console.Clear();
                        productServices.ShowAllProducts();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("\n--- Məhsulun Stokunu Artırın ---");
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

                    case "9":
                        Console.Clear();
                        Console.WriteLine("\n--- Müştəri Sifariş Tarixçəsi ---");
                        orderProduct.ShowOrdersByEmail();
                        break;

                    case "10":
                        Console.Clear();
                        Console.WriteLine("\n--- Sifarişi Ləğv Et ---");
                        orderProduct.CancelOrder(productServices);
                        break;

                    case "0":
                        Console.Clear();
                        isRunning = false;
                        Console.WriteLine("Proqramdan çıxıldı.");
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Yanlış seçim! Yenidən cəhd edin.");
                        break;
                }
            }
        }
    }
}
