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

            //Static routes examples
            //routes.MapRoute("", "Shop/OldAction", new {controller = "Home", action="Index"});
            //routes.MapRoute("", "Shop/{action}", new {controller = "Home"});
            //routes.MapRoute("", "X{controller}/{action}");
            //routes.MapRoute("MyRoute", "{controller}/{action}", new {controller = "Home", action = "Index"});
            //routes.MapRoute("", "Public/{controller}/{action}", new {controller = "Home", action = "Index"});

            
            //routes.MapRoute("", "Home/{action}/{id}/{*catchall}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new[] { "UrlAndRoutes.AddtionalControllers" });
            //routes.MapRoute("", "{controller}/{action}/{id}/{*catchall}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new[] { "UrlAndRoutes.Controllers" });

            routes.MapMvcAttributeRoutes();

            routes.MapRoute("", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                new[] {"UrlAndRoutes.Controllers"});

            //routes.MapRoute("ChromeRoute", "{*catchall}", new {controller = "Home", action = "Index"},
            //    new {customConstrain = new UserAgentConstrain("Chrome")}, new[] {"UrlAndRoutes.AddtionalControllers"});

            //routes.MapRoute("", "{controller}/{action}/{id}/{*catchall}", 
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional }, 
            //    new{controller ="^H.*", action="^Index$|^About$", httpMethod= new HttpMethodConstraint("GET"), 
            //        id=new CompoundRouteConstraint(new IRouteConstraint[]
            //        {
            //            new AlphaRouteConstraint(), 
            //            new MinLengthRouteConstraint(6), 
            //        })},
            //    new[] { "UrlAndRoutes.Controllers" });
        }
    }
}
