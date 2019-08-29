using OrderFruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFruit.ViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public long CreditCardNumber { get; set; }
    }
}