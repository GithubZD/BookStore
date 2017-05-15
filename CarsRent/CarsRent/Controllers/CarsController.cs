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
        public ActionResult Index()
        {
            var categroy = db.Categroys.ToList();
            var brand = db.Brands.ToList();
            var seat = db.SeatNums.ToList();
            ViewBag.Brand = brand;
            ViewBag.Categroy = categroy;
            ViewBag.SeatNum = seat;
            return View();
        }
        public ActionResult CarList(int? page, int? BrandID, int? CategroyID, int? seatNum, decimal? min, decimal? max)
        {
            if (page == null)
            {
                page = 1;
            }
            //IPagedList<Car> list = db.Cars.ToPagedList((int)page, 12);
            var list = db.Cars.ToList();
            if (BrandID != null)
            {
                list = list.Where(c => c.BrandId == BrandID).ToList();
            }
            if (CategroyID != null)
            {
                list = list.Where(c => c.CategroyId == CategroyID).ToList();
            }
            if (seatNum != null)
            {
                list = list.Where(c => c.SeatNumId == seatNum).ToList();
            }
            if (min != null && max != null)
            {
                list = list.Where(c => c.RentPrice >= min && c.RentPrice <= max).ToList();
            }
            else if (min == null && max != null)
            {
                list = list.Where(c => c.RentPrice >= max).ToList();
            }
            else if (min != null && max == null)
            {
                list = list.Where(c => c.RentPrice <= min).ToList();
            }
            return PartialView("_CarList", list.ToPagedList((int)page, 12));
        }
        // GET: Cars/Details/5

        [Authorize]
        public ActionResult EditOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.SingleOrDefault(u => u.LoginName == User.Identity.Name);
            if (user.RealName == null || user.Iphone == null || user.CardNumber == null)
            {
                return RedirectToAction("EditUserInfo", "User",new {id=user.UserId,errorMessage="你的信息不全，请完善你的信息！" });
            }

            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

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

        public ActionResult UserEvaluate(int? id)
        {
            ViewBag.UserEvaluate = db.Evaluates.Where(e => e.CarId == id && e.EvaluateContent != null).ToList();

            return PartialView("_UserEvaluate");
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
