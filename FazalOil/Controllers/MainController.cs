using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FazilOil.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/
        public ActionResult Welcome()
        {
            return Stock();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }

        public ActionResult Stock()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }

        public ActionResult Invoice()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }

        public ActionResult UpdateProduct()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }
        public ActionResult LogIn()
        {
            Session["LoginInStatus"] = false;
            return View();
        }
        public ActionResult DailyExpenses()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }
		public ActionResult Zakaat()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }
        public ActionResult Sales()
        {
            if (Session["LoginInStatus"] != null)
            {
                if ((bool)Session["LoginInStatus"] == true)
                {
                    return View();
                }
                else
                {
                    return View("LogIn");
                }
            }
            else
            {
                return View("LogIn");
            }
        }

        [HttpGet]
        public string Verify(string id, string pass)
        {
            if (id == "admin" && pass == "Awais@123")
            {
                Session["LoginInStatus"] = true;
                Session["usertype"] = "ceo";
                return "1";
            }
            else if (id == "bilal" && pass == "bilal786")
            {
                Session["LoginInStatus"] = true;
                Session["usertype"] = "staff";
                return "1";
            }
            else
            {
                return "0";
            }
        }

    }
}