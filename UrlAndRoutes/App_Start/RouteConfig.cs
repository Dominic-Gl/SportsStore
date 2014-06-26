using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using UrlAndRoutes.Infrastructure;

namespace UrlAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.MapMvcAttributeRoutes();

            //routes.MapRoute("Diskfile", "Content/StaticContent.html", new {controller = "Customer", action = "List"});
            routes.IgnoreRoute("Content/{filename}.html");

            routes.Add(new Route("sayhello", new CustomRouteHandler()));

            routes.Add(new LegacyRoute(
                "~/article/windows_31_Overview.html",
                "~/old/.net_10_class_library"
                ));



            routes.MapRoute("myroute", "{controller}/{action}",null,new []
            {
                "UrlAndRoutes.Controllers"
            });
            routes.MapRoute("myotherroute", "App/{action}",
                new { controller = "Home" }, new[]
            {
                "UrlAndRoutes.Controllers"
            });

          
        }
    }
}
