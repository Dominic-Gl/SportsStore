using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ControllersAndActions.Controllers
{
    public class BasicController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            string controller = requestContext.RouteData.Values["controller"] as string;
            string action = requestContext.RouteData.Values["action"] as string;

            requestContext.HttpContext.Response.Write("Controller: " + controller +   ", Action: " +  action);
        }
    }
}