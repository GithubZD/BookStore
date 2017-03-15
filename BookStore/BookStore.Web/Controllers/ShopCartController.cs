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
            decimal totle=0;
            foreach (var item in list)
            {
                totle += item.Book.Price * item.Count;
            }
            ViewBag.totle = totle;
            return View(list);
        }

        public ActionResult AddToCart(int? bookID,int? count,string ReturnUrl)
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


            ViewBag.TotilCount = count;
            return PartialView("_ShopCartSummary");
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