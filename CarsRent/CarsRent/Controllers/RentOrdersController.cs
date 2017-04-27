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
    public class RentOrdersController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: RentOrders
        public ActionResult Index()
        {
            var rentOrders = db.OrderDetails.Where(
                o=>o.Order.LoginName==User.Identity.Name&&o.Order.UserManager!=0).
                OrderByDescending(o=>o.Order.OrderTime).ToList();

            return View(rentOrders);
        }

        // GET: RentOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail rentOrder = db.OrderDetails.Find(id);
            if (rentOrder == null)
            {
                return HttpNotFound();
            }
            return View(rentOrder);
        }


        public ActionResult AddOrder(int? carID, int? count, DateTime? date, int? useDay)
        {
            
            if (carID == null)
            {
                var result = new {Status = 0};
                return Json(result);
            }

            var car = db.Cars.Find(carID);
            if (car != null)
            {
                var newOrder = new Order
                {
                    LoginName = User.Identity.Name,
                    OrderTime = DateTime.Now,
                    PayYesNo = 0,
                    Status = 0,
                    UserManager = 1,
                    AdminManager=1
                };
                 db.Orders.Add(newOrder);

                var user = db.Users.SingleOrDefault(u => u.LoginName == User.Identity.Name);
                var orderDetails = new OrderDetail {
                    CarId = (int)carID,
                    OrderId = newOrder.OrderId,
                    AwayTime =(DateTime)date,
                    Days = useDay,
                    Deposit = useDay*count * car.RentPrice*(decimal)1.2,
                    Money = useDay*count * car.RentPrice,
                    Number =(int)count,
                };
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();

                int id = orderDetails.OrderDetailsId;
                var result = new { Status = 1, id=id };
                return Json(result);
            }
            else {
                var result = new { Status = 2 };
                return Json(result);
            }

            
        }

        public ActionResult CancelOrder(int? recordID)
        {
            DateTime nowDate = DateTime.Now;
            var order = db.OrderDetails.Find(recordID);
            if (order == null)
            {
                var result = new { Status = 0 };
                return Json(result);
            }
            else {
                order.Order.PayYesNo = 2;
                order.Order.CancelTime = nowDate;

                DateTime d1 = Convert.ToDateTime(nowDate);

                DateTime d2 = Convert.ToDateTime(order.AwayTime);

                DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));

                DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));

                int days = (d4 - d3).Days;
                decimal? oneDayMoney = order.Money/order.Days;
                decimal? BreachMoney = 0;
                if (days >= 0)
                {
                    switch (days)
                    {
                        case 0:
                            BreachMoney = oneDayMoney * (decimal)0.5;
                            break;
                        case 1:
                            BreachMoney = oneDayMoney * (decimal)0.15;
                            break;
                        case 2:
                            BreachMoney = oneDayMoney * (decimal)0.10;
                            break;
                        case 3:
                            BreachMoney = oneDayMoney * (decimal)0.05;
                            break;
                        default:
                            break;
                    }
                    order.Compensation = BreachMoney;
                    db.SaveChanges();
                    var result = new { Status = 1 };
                    return Json(result);
                }
                else {
                    var result = new { Status = 2 };
                    return Json(result);
                }
            }
        }

        public ActionResult AllOrder()
        {
            var rentOrders = db.OrderDetails.Where(
                o => o.Order.LoginName == User.Identity.Name && o.Order.UserManager != 0).
                OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_OrderList", rentOrders);
        }
        public ActionResult IsPayIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                o => o.Order.LoginName == User.Identity.Name && o.Order.UserManager != 0 && o.Order.PayYesNo !=0).
                OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_IsPayIndex", rentOrders);
        }
        public ActionResult NoPayIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.LoginName == User.Identity.Name && o.Order.UserManager != 0 && o.Order.PayYesNo == 0).
                 OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_NoPayIndex", rentOrders);
        }
        public ActionResult IsCancleIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.LoginName == User.Identity.Name && o.Order.UserManager != 0 && o.Order.PayYesNo == 2).
                 OrderBy(o => o.Order.OrderTime).ToList();

            return PartialView("_IsCancleIndex", rentOrders);
        }
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

                order.UserManager = 0;
                db.SaveChanges();
                var result = new { Status = 1 };
                return Json(result);
            }

        }

        public ActionResult PayOrder(int? orderID)
        {
            if (orderID == null)
            {
                var result = new { Status = 0 };
                return Json(result);
            }
            var order = db.Orders.SingleOrDefault(o => o.OrderId == orderID);
            if (order != null)
            {
                order.PayYesNo = 1;
                db.SaveChanges();
                var result = new { Status = 1 };
                return Json(result);
            }
            else
            {
                var result = new { Status = 2 };
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
