using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsRent.Models;
using System.Windows.Forms;

namespace CarsRent.Controllers
{
    [Authorize]
    public class RentOrdersController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: RentOrders
        [Authorize]
        public ActionResult Index()
        {
            var rentOrders = db.OrderDetails.Where(
                o=>o.Order.User.LoginName==User.Identity.Name&&o.Order.UserManager==1).
                OrderByDescending(o=>o.Order.OrderTime).ToList();

            return View(rentOrders);
        }

        // GET: RentOrders/Details/5
        [Authorize]
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

        [HttpPost]
        public ActionResult AddOrder(int? carID, int? count, DateTime? date, int? useDay)
        {
                if (carID == null)
                {
                    var result = new { Status = 0 };
                    return Json(result);
                }

                var car = db.Cars.Find(carID);
                var user = db.Users.SingleOrDefault(u => u.LoginName == User.Identity.Name);
                if (car != null)
                {
                    var newOrder = new Order
                    {
                        UserId = user.UserId,
                        OrderTime = DateTime.Now,
                        PayYesNo = 0,
                        Status = 0,
                        Evaluate = 0,
                        UserManager = 1,
                        AdminManager = 1
                    };
                    db.Orders.Add(newOrder);
                    int num = (int)count;
                    if (car.NowNumber >= num)
                    {
                        car.NowNumber = car.NowNumber - num;
                        var orderDetails = new OrderDetail
                        {
                            CarId = (int)carID,
                            OrderId = newOrder.OrderId,
                            AwayTime = (DateTime)date,
                            Days = useDay,
                            Deposit = useDay * num * car.RentPrice * (decimal)1.2,
                            Money = useDay * num * car.RentPrice,
                            Number = num,
                        };
                        db.OrderDetails.Add(orderDetails);
                        db.SaveChanges();

                        int id = orderDetails.OrderDetailsId;
                        var result = new { Status = 1, id = id };
                        return Json(result);
                    }

                    else
                    {
                        var result = new { Status = 2 };
                        return Json(result);
                    }
                }
                else
                {
                    var result = new { Status = 0 };
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
                o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1).
                OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_OrderList", rentOrders);
        }
        public ActionResult IsPayIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1 && o.Order.PayYesNo !=0&&o.Order.Status==0).
                OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_IsPayIndex", rentOrders);
        }
        public ActionResult NoPayIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1 && o.Order.PayYesNo == 0).
                 OrderByDescending(o => o.Order.OrderTime).ToList();

            return PartialView("_NoPayIndex", rentOrders);
        }
        public ActionResult DealIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1 && o.Order.Status == 1).
                 OrderBy(o => o.Order.OrderTime).ToList();

            return PartialView("_DealIndex", rentOrders);
        }
        public ActionResult Complete()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1 && o.Order.Status == 2).
                 OrderBy(o => o.Order.OrderTime).ToList();

            return PartialView("_Complete", rentOrders);
        }
        public ActionResult IsCancleIndex()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager ==1 && o.Order.PayYesNo == 2).
                 OrderBy(o => o.Order.OrderTime).ToList();

            return PartialView("_IsCancleIndex", rentOrders);
        }
        public ActionResult NoEvaluate()
        {
            var rentOrders = db.OrderDetails.Where(
                 o => o.Order.User.LoginName == User.Identity.Name && o.Order.UserManager==1 && o.Order.Status == 2&&o.Order.Evaluate==0).
                 OrderBy(o => o.Order.OrderTime).ToList();

            return PartialView("_NoEvaluate", rentOrders);
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
        public ActionResult DeleteAll(string ids)
        {
            JsonResult json = new JsonResult();
            json.Data = false;
            ids = ids.Substring(0, ids.Length - 1);
            string[] id = ids.Split(',');
            for (int i = 0; i < id.Length; i++)
            {
                int recordId = int.Parse(id[i]);
                var order = db.Orders.SingleOrDefault(o => o.OrderId == recordId);
                order.UserManager = 0;
                db.SaveChanges();
                json.Data = true;
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
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
