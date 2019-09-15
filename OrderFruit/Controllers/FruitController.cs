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
    public class FruitController : Controller
    {
        private OrderFruitContext db = new OrderFruitContext();

        // GET: Fruit
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
                Session["Cart"] = new List<Fruit>();

            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            return View(new FruitViewModel() { Fruits = db.Fruit.ToList(), Cart = fruits });
        }

        public ActionResult Add(int id)
        {
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            switch (id)
            {
                case 1:
                    Fruit apple = new Fruit("Apple", 0.5, 0.15);
                    if(CheckIfFitsMoreWeight(fruits, apple.Weight))
                        fruits.Add(apple);
                    break;
                case 2:
                    Fruit orange = new Fruit("Orange", 0.2, 0.4);
                    if(CheckIfFitsMoreWeight(fruits, orange.Weight))
                        fruits.Add(orange);
                    break;
                case 3:
                    Fruit banana = new Fruit("Banana", 0.25, 0.35);
                    if(CheckIfFitsMoreWeight(fruits, banana.Weight))
                        fruits.Add(banana);
                    break;
            }
            return View("Index", new FruitViewModel(){ Fruits = db.Fruit.ToList(), Cart = fruits});
        }

        public ActionResult Remove(int fruitId)
        {
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            foreach (Fruit fruit in fruits)
            {
                if (fruit.Id == fruitId)
                {
                    fruits.Remove(fruit);
                    break;
                }
            }
            Session["Cart"] = fruits;
            return View("Index", new FruitViewModel() { Fruits = db.Fruit.ToList(), Cart = fruits });
        }

        public ActionResult Clear()
        {
            Session["Cart"] = new List<Fruit>();
            List<Fruit> fruits = (List<Fruit>)Session["Cart"];
            return View("Index", new FruitViewModel() { Fruits = db.Fruit.ToList(), Cart = fruits });
        }


        // POST: Fruit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Cost,Weight")] Fruit fruit)
        {
            if (ModelState.IsValid)
            {
                db.Fruit.Add(fruit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fruit);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CheckIfFitsMoreWeight(List<Fruit> fruits, double additionalWeight)
        {
            double weight = 0; 
            foreach (Fruit fruit in fruits)
            {
                weight += fruit.Weight;
            }

            weight += additionalWeight;
            return weight <= 3.0;
        }
    }
}
