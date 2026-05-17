using Mini_Project_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Repostories
{
    internal class ProductRepostory : Repostories<Product>
    {
        public ProductRepostory() : base(@"C:\Users\Asus\Desktop\Mini projecthm\Mini Project 1\Mini Project 1\Data\Product.json") { }
        
        protected override void SyncFromList(List<Product   > list)
        {
            Product.SyncCounter(list.Max(p => p.Id));
        }
               
    }
}
