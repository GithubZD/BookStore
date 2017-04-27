using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsRent.Models;
using X.PagedList.Mvc;
using X.PagedList;

namespace CarsRent.Controllers
{
    public class CarsController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: Cars
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            var categroy = db.Categroys.ToList();
            var brand = db.Brands.ToList();
            var cars = db.Cars.Include(c => c.Brand).Include(c => c.Categroy).Include(c => c.SeatNum).OrderBy(c=>c.CarId);
            ViewBag.Brand = brand;
            ViewBag.Categroy = categroy;
            ViewBag.Car = cars;
            return View(cars.ToPagedList((int)page, 12));
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName");
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumId");
            return View();
        }

        // POST: Cars/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,CategroyId,SeatNumId,BrandId,CarName,PlateNumber,RentPrice,Number,NowNumber,ImageUrl,Details")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", car.BrandId);
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName", car.CategroyId);
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumId", car.SeatNumId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult EditOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        
        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
