using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList.Mvc;
using X.PagedList;

namespace BookStore.Web.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        
        public ActionResult Index(int? pageNumber)
        {
            //string sql = "select top 30 * from(select top @pageNUmber*30 * from Books orderby BookId)orderby BookId dec";

            if (pageNumber == null)
            {
                pageNumber = 1;
            }
            using (BookStoreDB db = new BookStoreDB()) {
                IPagedList<Book> list = db.Books.OrderByDescending(
                p => p.BookId).ToPagedList((int)pageNumber, 30);
                return View(list);
            }
        }

    }
}