using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList.Mvc;
using X.PagedList;
using System.Net;

namespace BookStore.Web.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        private BookStoreDB db = new BookStoreDB();
        public ActionResult Index(int? pageNumber)
        {
            //string sql = "select top 30 * from(select top @pageNUmber*30 * from Books orderby BookId)orderby BookId dec";

            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            IPagedList<Book> list = db.Books.OrderByDescending(
            p => p.BookId).ToPagedList((int)pageNumber, 30);
            return View(list);

        }

        public ActionResult SearchBook(string bookName)
        {
            var list = db.Books.Where(b => b.Title.Contains(bookName));

            return View(list);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult ss()
        {
            return View(db.Books.ToList());
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