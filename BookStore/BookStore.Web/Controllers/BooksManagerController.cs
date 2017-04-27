using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Web.Models;
using X.PagedList.Mvc;
using X.PagedList;

namespace BookStore.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BooksManagerController : Controller
    {
        private BookStoreDB db = new BookStoreDB();

        // GET: Books
        public ActionResult Index(int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            var books = db.Books.Include(b => b.Author).Include(b => b.Category).OrderBy(b=>b.BookId);
            return View(books.ToPagedList((int)page,30));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name");
            ViewBag.CategroyId = new SelectList(db.Categorys, "CategoryId", "Name");
            return View();
        }

        // POST: Books/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,CategroyId,AuthorId,Title,Price,Details")] Book book,
            HttpPostedFileBase imageFile)
        {
            //HttpPostedFileBase postFile = Request.Files["imageFile"];
            if (ModelState.IsValid)
            {
                string imageName = Guid.NewGuid().ToString() + imageFile.FileName;
                string pathName=Server.MapPath("~/BookImages/"+ imageName);
                imageFile.SaveAs(pathName);
                book.ImageUrl = "/BookImages/" + imageName;
                db.Books.Add(book);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    return Content("错误是："+e);
                }
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId);
            ViewBag.CategroyId = new SelectList(db.Categorys, "CategoryId", "Name", book.CategroyId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId);
            ViewBag.CategroyId = new SelectList(db.Categorys, "CategoryId", "Name", book.CategroyId);
            return View(book);
        }

        // POST: Books/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,CategroyId,AuthorId,Title,Price,Details,ImageUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId);
            ViewBag.CategroyId = new SelectList(db.Categorys, "CategoryId", "Name", book.CategroyId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = db.Books.SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        // POST: Books/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Book book = db.Books.Find(id);
        //    db.Books.Remove(book);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
