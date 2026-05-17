using Mini_Project_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Repostories
{
    internal class OrdersRepostory : Repostories<Order>
    {
        public OrdersRepostory() : base(@"C:\Users\Asus\Desktop\Mini projecthm\Mini Project 1\Mini Project 1\Data\Order.json") { }
        protected override void SyncFromList(List<Order> list)
        {
            Order.SyncCounter(list.Max(p => p.Id));
        }
    }
}
