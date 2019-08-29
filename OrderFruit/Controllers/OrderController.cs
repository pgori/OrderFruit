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
            
            return View(db.Order.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            orderViewModel.Order = new Order();
            orderViewModel.Order.Customer = new Customer();
            orderViewModel.Order.Fruits = fruits;
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

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.Customer = orderViewModel.Order.Customer;
                order.Fruits = orderViewModel.Order.Fruits;
                order.TotalCost = orderViewModel.Order.TotalCost;
                order.TotalWeight = orderViewModel.Order.TotalWeight;

                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel orderViewModel)
        {
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.Customer = orderViewModel.Order.Customer;
                order.Fruits = fruits;
                order.TotalCost = orderViewModel.Order.TotalCost;
                order.TotalWeight = orderViewModel.Order.TotalWeight;

                db.Order.Add(order);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            fruits = new List<Fruit>();
            return View("~/Views/Fruit/Index.cshtml", new FruitViewModel() { Fruits = db.Fruit.ToList(), Cart = fruits });
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
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
