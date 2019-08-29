using OrderFruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.ViewModel
{
    public class FruitViewModel
    {
        public List<Fruit> Fruits { get; set; }
        public List<Fruit> Cart { get; set; }
    }
}