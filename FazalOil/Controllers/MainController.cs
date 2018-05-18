using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FazalOil.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/
        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult Stock()
        {
            return View();
        }

        public ActionResult Invoice()
        {
            return View();
        }

        public ActionResult UpdateProduct()
        {
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult DailyExpenses()
        {
            return View();
        }
		public ActionResult Zakaat()
        {
            return View();
        }
    }
}