﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    public class ShopCartController : Controller
    {
        // GET: ShopCart
        public ActionResult Index()
        {
            return View();
        }
    }
}