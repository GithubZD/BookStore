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
            var orderList = db.Orders.Where(o => o.AdminManager == 1&&o.User.RealName==OrderInfo).OrderByDescending(o=>o.OrderId).ToList();
            return PartialView("_OrderList", orderList);
        }
        public ActionResult OrderCategory(int? Pay,int? CarStatus)
        {
            if (CarStatus == null&& Pay==null)
            {
                    var orderList = db.Orders.Where(o => o.AdminManager == 1).OrderByDescending(o => o.OrderId).ToList();
                    return PartialView("_OrderList", orderList);
            }
            else
            {
                    var orderList = db.Orders.Where(o => o.AdminManager == 1&&o.PayYesNo== Pay && o.Status == CarStatus).OrderByDescending(o => o.OrderId).ToList();
                    return PartialView("_OrderList", orderList);
            }

        }
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
        public ActionResult UserGetCar(int? OrderId)
        {
            if (OrderId == null)
            {
                var result = new
                {
                    State = 0
                };

                return Json(result);
            }
            else
            {
                var order = db.Orders.SingleOrDefault(o => o.OrderId == OrderId);
                order.Status = 1;
                db.SaveChanges();
                var result = new
                {
                    State = 1
                };

                return Json(result);
            }
        }

        public ActionResult UserReturnCar(int? OrderId)
        {
            if (OrderId == null)
            {
                var result = new
                {
                    State = 0
                };

                return Json(result);
            }
            else
            {
                var order = db.OrderDetails.SingleOrDefault(o => o.Order.OrderId == OrderId);
                order.Order.Status = 2;
                var orderDetail = db.OrderDetails.SingleOrDefault(o => o.OrderId == OrderId);
                orderDetail.ReturnTime = DateTime.Now;
                var evaluate = new Evaluate {
                    OrderDetailsId= order.OrderDetailsId,
                    UserId=order.Order.UserId,
                    CarId=order.CarId
                };
                db.Evaluates.Add(evaluate);
                db.SaveChanges();
                var result = new
                {
                    State = 1
                };

                return Json(result);
            }
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
