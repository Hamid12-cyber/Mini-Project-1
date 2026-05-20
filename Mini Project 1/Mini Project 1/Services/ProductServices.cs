using Mini_Project_1.Models;
using Mini_Project_1.Enums;
using Mini_Project_1.Repostories;

namespace Mini_Project_1.Services
{
    internal class ProductServices
    {
        internal ProductRepostory ProductRepo { get; set; } = new();

        private static void Print(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void CreateProduct()
        {
            List<Product> products = ProductRepo.Deserialize();

            string name;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Ad: ");
                Console.ResetColor();
                name = Console.ReadLine()?.Trim() ?? "";

                if (name.Length == 0)
                { Print("  Ad boş ola bilməz!", ConsoleColor.Red); continue; }

                if (char.IsDigit(name[0]))
                { Print("  Adın ilk simvolu hərf olmalıdır!", ConsoleColor.Red); continue; }

                bool duplicate = false;
                foreach (Product p in products)
                {
                    if (p.Name.ToLower() == name.ToLower())
                    { Print($"  '{name}' adlı məhsul artıq mövcuddur!", ConsoleColor.Red); duplicate = true; break; }
                }
                if (duplicate) continue;
                break;
            }

            decimal price;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Qiymət (AZN): ");
                Console.ResetColor();
                string input = Console.ReadLine()?.Trim() ?? "";

                if (!decimal.TryParse(input, out price))
                { Print("  Düzgün rəqəm daxil edin! (məs: 12.50)", ConsoleColor.Red); continue; }

                if (price <= 0)
                { Print("  Qiymət sıfırdan böyük olmalıdır!", ConsoleColor.Red); continue; }

                break;
            }

            int stock;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Stok: ");
                Console.ResetColor();
                string input = Console.ReadLine()?.Trim() ?? "";

                if (!int.TryParse(input, out stock))
                { Print("  Düzgün tam ədəd daxil edin! (məs: 10)", ConsoleColor.Red); continue; }

                if (stock <= 0)
                { Print("  Stok ən az 1 ədəd daxil edilməlidir", ConsoleColor.Red); continue; }

                break;
            }

            ProductCategory category;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Kateqoriya seçin:");
                Console.ResetColor();
                Console.WriteLine("  1. Elektronika");
                Console.WriteLine("  2. Maşın");
                Console.WriteLine("  3. Digər");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Seçiminiz: ");
                Console.ResetColor();

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": category = ProductCategory.Elektronika; goto categoryDone;
                    case "2": category = ProductCategory.Maşın; goto categoryDone;
                    case "3": category = ProductCategory.Digər; goto categoryDone;
                    default: Print("  Yanlış seçim, 1-3 arasında daxil edin.", ConsoleColor.Red); break;
                }
            }
        categoryDone:

            Product newProduct = new Product(name, price, stock, category);
            products.Add(newProduct);
            ProductRepo.Serialize(products);

            Print("\n  Məhsul uğurla yaradıldı!", ConsoleColor.Yellow);
            Product.PrintHeader();
            newProduct.PrintInfo();
            Product.PrintFooter();
        }
        public void DeleteProduct()
        {
            int id;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Silinəcək ID: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out id)) break;
                Print("  Düzgün ID daxil edin.", ConsoleColor.Red);
            }

            List<Product> products = ProductRepo.Deserialize();
            Product productToDelete = null;
            foreach (var p in products)
                if (p.Id == id) { productToDelete = p; break; }

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
                ProductRepo.Serialize(products);
                Print($"\n  {id} nömrəli məhsul silindi.", ConsoleColor.Yellow);
            }
            else
            {
                Print($"\n  Bu ID-li məhsul tapılmadı!", ConsoleColor.Red);
            }
        }

        public void GetProductById()
        {
            List<Product> products = ProductRepo.Deserialize();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n  Axtarılacaq ID (geri üçün 0): ");
                Console.ResetColor();

                if (!int.TryParse(Console.ReadLine(), out int searchId))
                { Print("  Düzgün ID daxil edin.", ConsoleColor.Red); continue; }

                if (searchId == 0) break;

                Product? found = products.Find(p => p.Id == searchId);
                if (found != null)
                {
                    Print("\n  Məhsul Tapıldı:", ConsoleColor.Yellow);
                    Product.PrintHeader();
                    found.PrintInfo();
                    Product.PrintFooter();
                }
                else
                {
                    Print($"\n  {searchId} ID-li məhsul bazada yoxdur!", ConsoleColor.Red);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n  Yenidən axtar? (b - Bəli / x - Xeyr): ");
                Console.ResetColor();
                if (Console.ReadLine()?.Trim().ToLower() != "b") break;
            }
        }

        public void ShowAllProducts()
        {
            List<Product> products = ProductRepo.Deserialize();

            if (products.Count == 0)
            { Print("\n  Hazırda məhsul yoxdur.", ConsoleColor.Yellow); return; }

            foreach (ProductCategory cat in Enum.GetValues<ProductCategory>())
            {
                List<Product> group = products.Where(p => p.Category == cat).ToList();
                if (group.Count == 0) continue;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n  ══ {cat.ToString().ToUpper()} ══");
                Console.ResetColor();

                Product.PrintHeader();
                foreach (var p in group)
                    p.PrintInfo();
                Product.PrintFooter();
            }
        }

        public void RefillProduct()
        {
            int id;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Məhsulun ID-sini daxil edin: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out id)) break;
                Print("  Düzgün ID daxil edin.", ConsoleColor.Red);
            }

            int amount;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Artırılacaq miqdar: ");
                Console.ResetColor();
                if (!int.TryParse(Console.ReadLine(), out amount))
                { Print("  Düzgün tam ədəd daxil edin.", ConsoleColor.Red); continue; }
                if (amount <= 0)
                { Print("  Miqdar sıfırdan böyük olmalıdır.", ConsoleColor.Red); continue; }
                break;
            }

            List<Product> products = ProductRepo.Deserialize();
            Product? productToUpdate = products.Find(p => p.Id == id);

            if (productToUpdate == null)
            { Print("\n  Məhsul tapılmadı.", ConsoleColor.Red); return; }

            productToUpdate.Stock += amount;
            ProductRepo.Serialize(products);
            Print($"\n  '{productToUpdate.Name}' məhsulunun yeni stoku: {productToUpdate.Stock}", ConsoleColor.Yellow);
        }
    }
}