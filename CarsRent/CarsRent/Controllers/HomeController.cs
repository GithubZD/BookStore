using CarsRent.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsRent.Controllers
{
    public class HomeController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        
        public ActionResult Index()
        {
            //List<Car> list = db.Cars.Take(8).ToList();
            DateTime nowDate = DateTime.Now;
            DateTime d = new DateTime(2017, 04, 26, 00, 00, 00);
            //TimeSpan subtractDate = d1 - nowDate;
            int day = (d - nowDate).Days;


            string str1 = "2017-2-13 23:59:59";

            string str2 = "2017-2-14 0:00:01";

            DateTime d1 = Convert.ToDateTime(str1);

            DateTime d2 = Convert.ToDateTime(str2);

            DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));

            DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));

            int days = (d4 - d3).Days;

            return View();
        }
        [AllowAnonymous]
        public void CancelOrder()
        {
            DateTime nowDate = DateTime.Now;
            var rentOrders = db.OrderDetails.Where(o => o.Order.PayYesNo == 1).ToList();
            foreach (var item in rentOrders)
            {
                DateTime d1 = Convert.ToDateTime(nowDate);

                DateTime d2 = Convert.ToDateTime(item.AwayTime);

                DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));

                DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));

                int days = (d4 - d3).Days;
                if (days < 0)
                {
                    item.Order.Remark = "订单逾期";
                }
                db.SaveChanges();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ErrorMessage()
        {

            return View();
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