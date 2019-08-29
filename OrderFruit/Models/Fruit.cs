using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.Models
{
    public class Fruit
    {
        public Fruit()
        {

        }

        public Fruit(string name, double cost, double weight)
        {
            Name = name;
            Cost = cost;
            Weight = weight;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }
    }
}