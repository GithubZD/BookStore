using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace BookStore.Web.Controllers
{
    
    public class OrderController : Controller
    {
        private BookStoreDB db = new BookStoreDB();
        // GET: Order
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {          
            var list = db.Orders.OrderByDescending(o=>o.OrderDate).ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult CreateOrder(string ids)
        {
            Order order = new Order();
            ViewBag.checkList = ids;
            decimal price = 0;
            JsonResult json = new JsonResult();
            ids = ids.Substring(0, ids.Length - 1);
            string[] id = ids.Split(',');

            for (int i = 0; i < id.Length; i++)
            {
                int recordId = int.Parse(id[i]);
                var item = db.Carts.SingleOrDefault(c => c.RecordId == recordId);
                if (item != null)
                {
                    price += item.Book.Price * item.Count;
                }
            }

            order.Total = price;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(order);
        }

        [HttpPost]
        public ActionResult CreateOrder(Order order, string ids)
        {

            if (ModelState.IsValid)
            {

                JsonResult json = new JsonResult();
                ids = ids.Substring(0, ids.Length - 1);
                string[] id = ids.Split(',');

                var cartList = db.Carts.Where(c => c.CartId == User.Identity.Name).ToList();

                if (cartList != null && cartList.Count > 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.UserName = User.Identity.Name;
                    db.Orders.Add(order);

                    for (int i = 0; i < id.Length; i++)
                    {
                        int recordId = int.Parse(id[i]);
                        var item = db.Carts.SingleOrDefault(c => c.RecordId == recordId);
                        if (item != null)
                        {
                            var orderDeails = new OrderDetail()
                            {
                                BookId = item.BookId,
                                OrderId = order.OrderId,
                                Quantity = item.Count,
                                UnitPrice = item.Book.Price * item.Count
                            };
                            db.OrderDetails.Add(orderDeails);
                            db.Carts.Remove(item);
                        }
                        else {
                            MessageBox.Show("购物车中商品信息错误！");
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Complate");
                }
                else {
                    MessageBox.Show("数据异常，订单添加失败！");
                }
            }
            return View(order);
        }
        
        public ActionResult Complate()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.OrderDetails.Where(o => o.OrderId == id).ToList();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? recordID)
        {
            if (recordID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.SingleOrDefault(o =>o.OrderId == recordID);
            var orderDetails = db.OrderDetails.Where(o => o.OrderId == recordID).ToList();
            if (order == null|| orderDetails==null)
            {
                return HttpNotFound();
            }
            else
            {
                foreach (var item in orderDetails)
                {
                    db.OrderDetails.Remove(item);
                    db.SaveChanges();
                }
                db.Orders.Remove(order);
                db.SaveChanges();
                var result = new { Status = 1};
                return Json(result);
            }

        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAll(string ids)
        {
            JsonResult json = new JsonResult();
            json.Data = false;
            ids = ids.Substring(0, ids.Length - 1);
            string[] id = ids.Split(',');
            for (int i = 0; i < id.Length; i++)
            {
                // 调用删除方法
                //SecretaryCreater.BusinessManager.DelBusinessIntroduceByID(Guid.Parse(id[i]));
                json.Data = true;
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
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