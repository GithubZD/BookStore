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
        
        [HttpPost]
        public ActionResult Delete(int? recordID,string ids)
        {
            if (recordID == null)
            {
                var result = new { Status = 0 };
                return Json(result);
            }
            var item = db.Carts.SingleOrDefault(c => c.RecordId == recordID && c.CartId == User.Identity.Name);
            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();

                var total = GetTotal();
                decimal price = UpdatePrice(ids);
                var result = new {
                    Status = 1,
                    TotalPrice=price,
                    TotalCount=total.Item2};
                return Json(result);
            }
            else
            {
                var result = new { Status = 2};
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
                int recordId =int.Parse(id[i]);
                var item = db.Carts.SingleOrDefault(c => c.RecordId == recordId);
                db.Carts.Remove(item);
                db.SaveChanges();
                json.Data = true;
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCount(int? recordID,int? count,string ids)
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
                decimal price = UpdatePrice(ids);
                var result = new
                {
                    Status = 1,
                    TotalPrice = price,
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

        public decimal UpdatePrice(string ids)
        {

            decimal price = 0;

            if (ids.Length>0) {
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
            }

            return price;

        }
        public ActionResult UpdateTotalPrice(string ids)
        {
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
            var reslut = new { TotalPrice = price };
            return Json(reslut);
            


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