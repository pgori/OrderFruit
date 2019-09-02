using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public List<FruitOrder> FruitOrder { get; set; }
        public double TotalCost { get; set; }
        public double TotalWeight { get; set; }
    }
}