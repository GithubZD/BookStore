using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private BookStoreDB db = new BookStoreDB();
        // GET: ShopCart
        public ActionResult Index()
        {
           var list= db.Carts.Where(
               c => c.CartId == User.Identity.Name).OrderByDescending(
               c=>c.DeteCreated).ToList();
            decimal price=0;
            foreach (var item in list)
            {
                price += item.Book.Price * item.Count;
            }
            ViewBag.totalPrice = price;
            return View(list);
        }

        public ActionResult AddToCart(int? bookID,int? count)
        {
            if (bookID == null||count==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = db.Books.Find(bookID);
            if (book!=null)
            {
                var cart=  db.Carts.SingleOrDefault(c=>c.BookId==bookID&&c.CartId==User.Identity.Name);
                if (cart!=null)
                {
                    cart.Count +=(int)count;
                }
                else
                {
                    var newCart = new Cart() {
                    BookId=(int)bookID,
                    CartId=User.Identity.Name,
                    Count=(int)count,
                    DeteCreated=DateTime.Now};
                    db.Carts.Add(newCart);

                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult GetShopCartSummary()
        {
            int count = 0;
            if (User.Identity.IsAuthenticated)
            {
                var list = db.Carts.Where(
                    c => c.CartId == User.Identity.Name).ToList();

                foreach (var item in list)
                {
                    count += item.Count;
                }
            }


            ViewBag.TotalCount = count;
            return PartialView("_ShopCartSummary");
        }
        
        //[HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                var result = new { Status = 0 };
                return Json(result);
            }
            var item = db.Carts.SingleOrDefault(c => c.BookId == id && c.CartId == User.Identity.Name);
            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();

                var total = GetTotal();
                var result = new {
                    Status = 1,
                    TotalPrice=total.Item1,
                    TotalCount=total.Item2};
                return Json(result);
            }
            else
            {
                var result = new { Status = 2};
                return Json(result);
            }

        }

        
        public ActionResult UpdateCount(int? recordID,int? count)
        {
            if (recordID==null||count==null)
            {
                var result = new { Status = 0 };
                return Json(result);
            }
            var cart = db.Carts.SingleOrDefault(c => c.RecordId == recordID && c.CartId==User.Identity.Name);
            if (cart!=null)
            {
                cart.Count = (int)count;
                db.SaveChanges();

                var total = GetTotal();
                var result = new
                {
                    Status = 1,
                    TotalPrice = total.Item1,
                    TotalCount = total.Item2
                };
                return Json(result);
            }
            else
            {
                var result = new { Status = 2 };
                return Json(result);
            }

        }
        public Tuple<decimal,int> GetTotal()
        {

            var list = db.Carts.Where(
                c => c.CartId == User.Identity.Name).ToList();

            decimal price = 0;
            int count = 0;
            foreach (var item in list)
            {
                price += item.Count * item.Book.Price;
                count += item.Count;
            }
            return new Tuple<decimal, int>(price, count);
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