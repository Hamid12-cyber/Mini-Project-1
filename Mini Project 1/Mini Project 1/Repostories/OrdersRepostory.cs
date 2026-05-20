using Mini_Project_1.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mini_Project_1.Repostories
{
    internal class OrdersRepostory : Repostories<Order>
    {
        public OrdersRepostory() : base(@"Mini projecthm\Mini Project 1\Mini Project 1\Data\Order.json") { }

        protected override void SyncFromList(List<Order> list)
        {
            if (list != null && list.Count > 0)
            {
                Order.SyncCounter(list.Max(p => p.Id));
            }
        }
    }
}