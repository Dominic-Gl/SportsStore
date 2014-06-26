using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace UrlAndRoutes.Infrastructure
{
    public class CustomRouteHandler :IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CustomHttpHandler();
        }
    }

    public class CustomHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("hello");
        }

        public bool IsReusable { get { return false; } }
    }
}