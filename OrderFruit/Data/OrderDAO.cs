using OrderFruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.Data
{
    public class OrderDAO
    {
        public void Add(Order order)
        {
            using (var context = new OrderFruitContext())
            {
                context.Order.Add(order);
                context.SaveChanges();
            }
        }
    }
}