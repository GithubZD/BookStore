﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CarsRent.Models;
using NIIT.BookStore.Web.Controllers;
using System.Net;
using System.Data.Entity;
using CarsRent.Common;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace CarsRent.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private CarsRentDB db = new CarsRentDB();

        public ActionResult Index()
        {
            var user = db.Users.SingleOrDefault(u => u.LoginName == User.Identity.Name);

            return View(user);
        }

        public ActionResult GetUserIcon()
        {
            var user = db.Users.SingleOrDefault(u=>u.LoginName==User.Identity.Name);
            if (user.Icon == null){
                ViewBag.UserIcon = "/Images/IconImg/happy_face.png";
            }
            else
            {
                ViewBag.UserIcon = user.Icon;
            }
            return PartialView("_UserIcon");
        }
        public ActionResult UserInfo(int id)
        {
            var user = db.Users.Find(id);

            return PartialView("_UserInfo", user);
        }
        public ActionResult EditUserInfo(int? id)
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

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfo([Bind(Include = "UserId,Icon,LoginName,RealName,Password,CardNumber,Iphone,Age,Sex,Address,RoleId")] User user)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = db.Users.SingleOrDefault(u => u.LoginName == User.Identity.Name);
            string oldPassword = SecurityPsw.SHA1PAssword(model.OldPassword);
            if (user.Password != oldPassword)
            {
                ModelState.AddModelError("OldPassword", "原密码错误");
            }
            else
            {
                string newPassword = SecurityPsw.SHA1PAssword(model.NewPassword);
                user.Password = newPassword;
                db.SaveChanges();
                MessageBox.Show("修改密码成功，请重新登陆！");
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult AddIcon(int? UserID, string iconString)
        {
            if (iconString == null|| UserID==null)
            {
                var result = new
                {
                    status = 0
                };
            return Json(result);
            }
            else
            {
                var user = db.Users.Find(UserID);
                string dummyData = iconString.Trim().Replace("data:image/jpeg;base64,", "");
                string imageName = Guid.NewGuid().ToString()+ ".jpg";
                string pathName = Server.MapPath("~/Images/IconImg/");
                //byte[] arr2 = Encoding.UTF8.GetBytes(iconString);
                byte[] arr2 = Convert.FromBase64String(dummyData);
                using (MemoryStream ms2 = new MemoryStream(arr2))
                {
                    System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                    bmp2.Save(pathName + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                user.Icon = "/Images/IconImg/" + imageName;
                db.SaveChanges();
                var result = new
                {
                    status = 1
                };
                return Json(result);
            }
        }



   

   

        //
        // GET: /Manage/Index
       



        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
                db = null;
            }

            base.Dispose(disposing);
        }

#region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}