using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UrlAndRoutes.Tests
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            // create the mock request
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            // create the mock response
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            // create the mock context, unsing the request and response
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, object routeProperties = null,
            string httpMethod = "GET")
        {
            // Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Asseret
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) => StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;

            bool result = valCompare(routeResult.Values["controller"], controller) &&
                          valCompare(routeResult.Values["action"], action);

            if(propertySet != null)
            {
                PropertyInfo[] propertyInfos = propertySet.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if(!(routeResult.Values.ContainsKey(propertyInfo.Name) && valCompare(routeResult.Values[propertyInfo.Name], propertyInfo.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private void TestRouteFail(string url)
        {
            // arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // act -  process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            Assert.IsTrue(result == null || result.Route == null);
        }

        //[TestMethod]
        //public void TestIncomingRoutes()
        //{
            
        //    TestRouteMatch("~/", "Home", "Index");
        //    TestRouteMatch("~/Home", "Home" ,"Index");
        //    TestRouteMatch("~/Home/Index", "Home", "Index");

        //    TestRouteMatch("~/Home/About", "Home", "About");
        //    TestRouteMatch("~/Home/About/MyId", "Home", "About" ,new{id="MyId"});
        //    TestRouteMatch("~/Home/About/MyId/More/Segments", "Home", "About" ,new{id="MyId", catchall="More/Segments"});

        //    TestRouteFail("~/Home/OtherAction");
        //    TestRouteFail("~/Account/Index");
        //    TestRouteFail("~/Account/About");
        //}
    }
}
