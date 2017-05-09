using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsRent.Models;

namespace CarsRent.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerOrderController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: MangerOrder
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult OrderList( string OrderInfo)
        {
            var orderList = db.Orders.Where(o => o.AdminManager == 1).OrderByDescending(o=>o.OrderId).ToList();
            return PartialView("_OrderList", orderList);
        }
        public ActionResult OrderCategory(int? Pay,int? CarStatus)
        {
            if (CarStatus == null)
            {
                if (Pay == null)
                {
                    var orderList = db.Orders.Where(o => o.AdminManager == 1).OrderByDescending(o => o.OrderId).ToList();
                    return PartialView("_OrderList", orderList);
                }
                else
                {
                    var orderList = db.Orders.Where(o => o.AdminManager == 1 && o.PayYesNo == Pay).OrderByDescending(o => o.OrderId).ToList();
                    return PartialView("_OrderList", orderList);
                }
            }
            else
            {
                    var orderList = db.Orders.Where(o => o.AdminManager == 1&&o.PayYesNo!=0 && o.Status == CarStatus).OrderByDescending(o => o.OrderId).ToList();
                    return PartialView("_OrderList", orderList);

            }

        }
        public ActionResult OrderCarStatus(int? CarStatus)
        {
            if (CarStatus == null)
            {
                var orderList = db.Orders.Where(o => o.AdminManager == 1).OrderByDescending(o => o.OrderId).ToList();
                return PartialView("_OrderList", orderList);
            }
            else
            {
                var orderList = db.Orders.Where(o => o.AdminManager == 1 && o.Status == CarStatus).OrderByDescending(o => o.OrderId).ToList();
                return PartialView("_OrderList", orderList);
            }
        }
        // GET: MangerOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail order = db.OrderDetails.SingleOrDefault(o=>o.OrderId==id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: MangerOrder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MangerOrder/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderTime,LoginName,PayYesNo,Status,UserManager,AdminManager")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: MangerOrder/Edit/5
        
        public ActionResult Delete(int? recordID)
        {
            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.SingleOrDefault(o => o.OrderId == recordID);
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {

                order.AdminManager = 0;
                db.SaveChanges();
                var result = new { Status = 1 };
                return Json(result);
            }

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
