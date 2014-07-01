using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using ControllersAndActions.Infrastructure;

namespace ControllersAndActions.Controllers
{
    public class DerivedController : Controller
    {
        // GET: Derived
        public ActionResult Index()
        {
            ViewBag.Message = " Hello from the derived controller index method";
            
            return View("MyView");
        }

        public ActionResult ProduceOutput()
        {
            
                return new RedirectResult("/Basic/index");
            
        }
    }
}