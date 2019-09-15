using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderFruit.Data;
using OrderFruit.Models;
using OrderFruit.ViewModel;

namespace OrderFruit.Controllers
{
    public class OrderController : Controller
    {
        private OrderFruitContext db = new OrderFruitContext();

        // GET: Order
        public ActionResult Index()
        {
            if(Session["Cart"] == null)
                Session["Cart"] = new List<Fruit>();
            
            return View();
        }

        // GET: Order/Details/5
        public ActionResult Details(Order orderModel)
        {
            if (orderModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order
                .Include(o => o.Customer)
                .Include(o => o.FruitOrder.Select(f => f.Fruit))
                .Where(o => o.Id == orderModel.Id)
                .FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View("Index", order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            orderViewModel.Order = new Order();
            orderViewModel.Order.Customer = new Customer();
            orderViewModel.Order.FruitOrder = new List<FruitOrder>();
            foreach (Fruit fruit in fruits)
            {
                FruitOrder fo = new FruitOrder();
                fo.Fruit = fruit;
                fo.Order = orderViewModel.Order;
                orderViewModel.Order.FruitOrder.Add(fo);
            }
            orderViewModel.Order.TotalCost = GetTotalCost(fruits);
            orderViewModel.Order.TotalWeight = GetTotalWeight(fruits);
            return View(orderViewModel);
        }

        private double GetTotalCost(List<Fruit> fruits)
        {
            double total = 0;
            foreach (Fruit fruit in fruits)
            {
                total += fruit.Cost;
            }
            return total;
        }

        private double GetTotalWeight(List<Fruit> fruits)
        {
            double total = 0;
            foreach (Fruit fruit in fruits)
            {
                total += fruit.Weight;
            }
            return total;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel orderViewModel)
        {
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.FruitOrder = new List<FruitOrder>();
                order.Customer = orderViewModel.Order.Customer;
                foreach (Fruit fruit in fruits)
                {
                    FruitOrder fo = new FruitOrder();
                    fo.Fruit = db.Fruit.FirstOrDefault(f => f.Name == fruit.Name);
                    fo.Order = null;
                    order.FruitOrder.Add(fo);
                }
                order.TotalCost = orderViewModel.Order.TotalCost;
                order.TotalWeight = orderViewModel.Order.TotalWeight;

                db.Order.Add(order);
                db.SaveChanges();
            }
            fruits = new List<Fruit>();
            Session["Cart"] = fruits;
            return RedirectToAction("Index", "Fruit", new FruitViewModel() { Fruits = db.Fruit.ToList(), Cart = fruits });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
