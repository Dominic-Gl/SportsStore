using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            //Static routes examples
            //routes.MapRoute("", "Shop/OldAction", new {controller = "Home", action="Index"});
            //routes.MapRoute("", "Shop/{action}", new {controller = "Home"});
            //routes.MapRoute("", "X{controller}/{action}");
            //routes.MapRoute("MyRoute", "{controller}/{action}", new {controller = "Home", action = "Index"});
            //routes.MapRoute("", "Public/{controller}/{action}", new {controller = "Home", action = "Index"});

            //Custom segment variables
        }
    }
}
