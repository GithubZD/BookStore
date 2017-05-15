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
    [Authorize(Roles = "Admin")]
    public class ManagerUserController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        // GET: Users
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult UserList( string UserInfo)
        {
            if (UserInfo == null)
            {
                var userList = db.Users.Where(u => u.RoleId==2).ToList();
                return PartialView("_UserList", userList);
            }
            else
            {
                var userList = db.Users.Where(u => u.RoleId == 2 && u.RealName==UserInfo||u.Iphone==UserInfo).ToList();
                return PartialView("_UserList", userList);
            }
        }
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
       
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
