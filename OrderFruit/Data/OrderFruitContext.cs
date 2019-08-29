using OrderFruit.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OrderFruit.Data
{
    public class OrderFruitContext : DbContext
    {
        public DbSet<Order> Order { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Fruit> Fruit { get; set; }
    }
}