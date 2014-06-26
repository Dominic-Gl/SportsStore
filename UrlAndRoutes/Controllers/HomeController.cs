using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlAndRoutes.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View("ActionName");
        } 

        public ActionResult CustomVariable(string id = "DefaultId")
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "CustomVariable";
            ViewBag.CustomVariable = id;

            return View();
        }

        public RedirectToRouteResult MyAction()
        {
            return RedirectToRoute(new {controller = "Home", action = "Index", id = "id"});
        }
	}
}