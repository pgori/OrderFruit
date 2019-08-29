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
                    Fruit orange = new Fruit("Orange", 0.4, 0.2);
                    if(CheckIfFitsMoreWeight(fruits, orange.Weight))
                        fruits.Add(orange);
                    break;
                case 3:
                    Fruit banana = new Fruit("Banana", 0.35, 0.25);
                    if(CheckIfFitsMoreWeight(fruits, banana.Weight))
                        fruits.Add(banana);
                    break;
            }
            //return View("~/Views/Fruit/Index.cshtml", db.Fruit.ToList());
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

        // GET: Fruit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fruit fruit = db.Fruit.Find(id);
            if (fruit == null)
            {
                return HttpNotFound();
            }
            return View(fruit);
        }

        // GET: Fruit/Create
        public ActionResult Create()
        {
            return View();
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

        // GET: Fruit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fruit fruit = db.Fruit.Find(id);
            if (fruit == null)
            {
                return HttpNotFound();
            }
            return View(fruit);
        }

        // POST: Fruit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Cost,Weight")] Fruit fruit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fruit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fruit);
        }

        // GET: Fruit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fruit fruit = db.Fruit.Find(id);
            if (fruit == null)
            {
                return HttpNotFound();
            }
            return View(fruit);
        }

        // POST: Fruit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fruit fruit = db.Fruit.Find(id);
            db.Fruit.Remove(fruit);
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
