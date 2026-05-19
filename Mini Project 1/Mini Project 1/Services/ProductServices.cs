using Mini_Project_1.Animations;
using Mini_Project_1.Models;
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

        public void CreateProduct(string name, decimal price, int stock)
        {
            List<Product> products = ProductRepo.Deserialize();
            foreach (Product p in products)
            {
                if (p.Name.ToLower() == name.Trim().ToLower())
                    throw new ArgumentException($"{name} adlı məhsul artıq müvcuddur. Birdaha sınayın");
            }

            if (price <= 0 || stock < 0)
            {
                Print("Qiymət 0-dan böyük, stok mənfi olmamalıdır!", ConsoleColor.Red);
                return;
            }

            Product newProduct = new Product(name, price, stock);
            products.Add(newProduct);
            ProductRepo.Serialize(products);

            Print($"\n  ✓ Məhsul uğurla yaradıldı!", ConsoleColor.Yellow);
            Print($"  ─────────────────────", ConsoleColor.DarkRed);
            Print($"  Ad    : {newProduct.Name}", ConsoleColor.White);
            Print($"  Qiymət: {newProduct.Price} AZN", ConsoleColor.White);
            Print($"  Stok  : {newProduct.Stock}", ConsoleColor.White);
            Print($"  ─────────────────────", ConsoleColor.DarkRed);
        }

        public void DeleteProduct(int id)
        {
            List<Product> products = ProductRepo.Deserialize();
            Product productToDelete = null;

            foreach (var p in products)
            {
                if (p.Id == id) { productToDelete = p; break; }
            }

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
                ProductRepo.Serialize(products);
                Print($"\n  ✓ {id} nömrəli məhsul silindi.", ConsoleColor.Yellow);
            }
            else
            {
                Print($"\n  ✗ Bu ID-li məhsul tapılmadı!", ConsoleColor.Red);
            }
        }

        public void GetProductById(int id)
        {
            List<Product> products = ProductRepo.Deserialize();
            Product? foundProduct = products.Find(p => p.Id == id);

            if (foundProduct != null)
            {
                Print("\n  ─── Məhsul Tapıldı ───", ConsoleColor.DarkRed);
                foundProduct.PrintInfo();
                Print("  ──────────────────────", ConsoleColor.DarkRed);
            }
            else
            {
                Print($"\n  ✗ {id} ID-li məhsul bazada yoxdur!", ConsoleColor.Red);
            }
        }

        public void ShowAllProducts()
        {
            List<Product> products = ProductRepo.Deserialize();

            if (products.Count == 0)
            {
                Print("\n  Hazırda məhsul yoxdur.", ConsoleColor.Yellow);
                return;
            }

            Print("\n  ─── Bütün Məhsullar ───", ConsoleColor.DarkRed);
            foreach (var p in products)
            {
                p.PrintInfo();
                Print("  ──────────────────────", ConsoleColor.DarkRed);
            }
        }

        public void RefillProduct(int id, int amount)
        {
            if (amount <= 0)
            {
                Print("\n  ✗ Daxil olunan stok 0 və ya mənfi ola bilməz.", ConsoleColor.Red);
                return;
            }

            List<Product> products = ProductRepo.Deserialize();
            Product? productToUpdate = products.Find(p => p.Id == id);

            if (productToUpdate == null)
            {
                Print("\n  ✗ Məhsul tapılmadı.", ConsoleColor.Red);
                return;
            }

            productToUpdate.Stock += amount;
            ProductRepo.Serialize(products);
            Print($"\n  ✓ '{productToUpdate.Name}' məhsulunun yeni stoku: {productToUpdate.Stock}", ConsoleColor.Yellow);
        }
    }
}
