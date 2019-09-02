using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.Models
{
    public class FruitOrder
    {
        public int Id { get; set; }

        public virtual Order Order { get; set; }
        public virtual Fruit Fruit { get; set; }
    }
}