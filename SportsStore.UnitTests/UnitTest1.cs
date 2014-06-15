using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(mockRepo => mockRepo.Products).Returns(new Product[]
            {
                new Product(){ProductID = 1, Name = "P1"},
                new Product(){ProductID = 2, Name = "P2"},
                new Product(){ProductID = 3, Name = "P3"},
                new Product(){ProductID = 4, Name = "P4"},
                new Product(){ProductID = 5, Name = "P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            ProductsListViewModel model = (ProductsListViewModel)controller.List(2).Model;

            IEnumerable < Product > result = model.Products;
            //assert
            Product[] prodArray = result.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //arrange
            //define an helper, we need to do this in order to apply the extensionmethod
            HtmlHelper myHelper = null;

            //arrange paginginfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // arrange- set lambda
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
                @"<a class=""btn btn-default"" href=""Page3"">3</a>" 
            ,result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(mockRepo => mockRepo.Products).Returns(new Product[]
            {
                new Product(){ProductID = 1, Name = "P1"},
                new Product(){ProductID = 2, Name = "P2"},
                new Product(){ProductID = 3, Name = "P3"},
                new Product(){ProductID = 4, Name = "P4"},
                new Product(){ProductID = 5, Name = "P5"}
            });

            //arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel) controller.List(2).Model;

            //assert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
    }
}
