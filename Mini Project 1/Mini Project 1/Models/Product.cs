using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Mini_Project_1.Models
{
    internal class Product
    {
        private string _name;
        private decimal _price;
        private int _stock;
        private static int _idCounter = 1;
        public string Name 
        { 
            get { return _name; } 
            set 
            {
                if (value.Trim().Length < 1)
                    throw new ArgumentException("Məhsulun adı ən azı 1 simvol olmalıdır.");
                _name = char.ToUpper(value[0]) + value.Substring(1).ToLower().Trim();
            }
        }
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Qiymət sıfırdan böyük olmalıdır.");
                }
                else
                {
                    _price = value;
                }
            }
        }
        public int Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Yanlış rəqəm daxil etdiniz.");
                }
                else if (value == 0)
                {
                    throw new ArgumentException("Məhsul stokda yoxdur."); 
                }
                else
                {
                    _stock = value;
                }
            }
        }
        public int Id { get; set; }
        public Product() { }
        public Product(string name, decimal price, int stock, List<string> existingNames)
        {
            
            if (existingNames.Any(n => n.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"'{name.Trim()}' adlı məhsul artıq mövcuddur.");

            Id = _idCounter++;
            Name = name;   
            Price = price;  
            Stock = stock;  
        }
        public static void SyncCounter(int maxExistingId)
        {
            if (maxExistingId >= _idCounter)
                _idCounter = maxExistingId + 1;
        }
                      
        public void PrintInfo() 
        {
            Console.WriteLine($"  Məhsulun adı : {Name}");
            Console.WriteLine($"  Qiyməti : {Price}Azn");
            Console.WriteLine($"  Barkod nömrəsi : {Id}");
            Console.WriteLine($"  Stok miqdarı : {Stock}");
        }

    }
    
}