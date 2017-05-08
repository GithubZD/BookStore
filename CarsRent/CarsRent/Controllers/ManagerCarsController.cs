using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsRent.Models;
using X.PagedList;

namespace CarsRent.Controllers
{
    public class ManagerCarsController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: ManagerCars
        public ActionResult Index()
        {
            var categroy = db.Categroys.ToList();
            var brand = db.Brands.ToList();
            ViewBag.Brand = brand;
            ViewBag.Category = categroy;
            return View();
        }
        public ActionResult ListCar(int? page,string carName)
        {
            if (page == null)
            {
                page = 1;
            }
            if (carName == null)
            {
                var cars = db.Cars.OrderByDescending(c => c.CarId);
                return PartialView("_ListCar", cars.ToPagedList((int)page, 12));
            }
            else
            {
                var cars = db.Cars.Where(c => c.CarName.Contains(carName)).OrderByDescending(c => c.CarId);
                return PartialView("_ListCar", cars.ToPagedList((int)page, 12));
            }
        }
        public ActionResult ListCarCheck(int? page, int? BrandID, int? CategroyID)
        {
            if (page == null)
            {
                page = 1;
            }
            var list = db.Cars.OrderByDescending(c => c.CarId).ToList();
            if (BrandID != null)
            {
                list = list.Where(c => c.BrandId == BrandID).OrderByDescending(c => c.CarId).ToList();
            }
            if (CategroyID != null)
            {
                list = list.Where(c => c.CategroyId == CategroyID).OrderByDescending(c => c.CarId).ToList();
            }
           return PartialView("_ListCar", list.ToPagedList((int)page, 12));
        }

        // GET: ManagerCars/Details/5
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

        // GET: ManagerCars/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName");
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumber");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,CategroyId,SeatNumId,BrandId,CarName,PlateNumber,RentPrice,Number,Details")] Car car,
            HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string imageName = Guid.NewGuid().ToString() + imageFile.FileName;
                    string pathName = Server.MapPath("~/Images/CarImg/" + imageName);
                    imageFile.SaveAs(pathName);
                    car.ImageUrl = "/Images/CarImg/" + imageName;
                    db.Cars.Add(car);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", car.BrandId);
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName", car.CategroyId);
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumber", car.SeatNumId);
            return View(car);
        }

        // GET: ManagerCars/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", car.BrandId);
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName", car.CategroyId);
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumber", car.SeatNumId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarId,CategroyId,SeatNumId,BrandId,CarName,PlateNumber,RentPrice,Number,Details")] Car car,
             HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string imageName = Guid.NewGuid().ToString() + imageFile.FileName;
                    string pathName = Server.MapPath("~/Images/CarImg/" + imageName);
                    imageFile.SaveAs(pathName);
                    car.ImageUrl = "/Images/CarImg/" + imageName;
                    db.Entry(car).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", car.BrandId);
            ViewBag.CategroyId = new SelectList(db.Categroys, "CategoryId", "CategoryName", car.CategroyId);
            ViewBag.SeatNumId = new SelectList(db.SeatNums, "SeatNumId", "SeatNumber", car.SeatNumId);
            return View(car);
        }

        // GET: ManagerCars/Delete/5
        [HttpPost]
        public ActionResult Delete(int? recordID)
        {
            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                Car car = db.Cars.Find(recordID);
                if (car == null)
                {
                    return HttpNotFound();
                }
                else
                {   
                    db.Cars.Remove(car);
                    db.SaveChanges();

                    var result = new
                    {
                        Status = 1,
                    };
                    return Json(result);
                }
            }
            
        }

        public ActionResult CreateCategroy()
        {
            return View();
         }

        public ActionResult CreateCategroy([Bind(Include = "CategoryId,CategoryName")] Categroy categroy)
        {
            if (ModelState.IsValid)
            {
                db.Categroys.Add(categroy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categroy);
        }

        public ActionResult CreateBrand()
        {
            return View();
        }

        public ActionResult CreateBrand([Bind(Include = "BrandId,BrandName")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        public ActionResult CreateSeatNum()
        {
            return View();
        }

        public ActionResult CreateSeatNum([Bind(Include = "SeatNumId,SeatNumber")] SeatNum seatNum)
        {
            if (ModelState.IsValid)
            {
                db.SeatNums.Add(seatNum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seatNum);
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
