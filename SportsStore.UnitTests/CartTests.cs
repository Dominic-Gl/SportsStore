using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product(){ProductID = 1, Name = "P1"};
            Product p2 = new Product(){ProductID = 2, Name = "P2"};

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] result= 
            target.Lines.ToArray();
            
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product(){ProductID = 1, Name = "P1"};
            Product p2 = new Product(){ProductID = 2, Name = "P2"};

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            CartLine[] result= 
            target.Lines.OrderBy(cart=> cart.Product.ProductID).ToArray();
            
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 11);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange
            Product p1 = new Product(){ProductID = 1, Name = "P1"};
            Product p2 = new Product(){ProductID = 2, Name = "P2"};
            Product p3 = new Product(){ProductID = 3, Name = "P3"};

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            //act
            target.RemoveLine(p2);
            
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Arrange
            Product p1 = new Product(){ProductID = 1, Name = "P1" , Price = 10M};
            Product p2 = new Product() { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);

            //act
            target.Clear();
            
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product(){ProductID = 1, Name = "P1", Category = "Apples"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object, null);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product(){ProductID = 1, Name = "P1", Category = "Apples"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object, null);

            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //Arrange 
            Cart cart = new Cart();

            CartController target = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreSame(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mockProc = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mockProc.Object);

            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert - check that the order hasn't been passed on to the processor, means that ProcessorOrder was never invoked
            mockProc.Verify(mock=> mock.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);
            //Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            //Assert- check that i am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mockProc = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(null, mockProc.Object);

            //arrange - add an error to the model
            target.ViewData.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //Assert - check that the order hasn't been passed on to the processor, means that ProcessorOrder was never invoked
            mockProc.Verify(mock=> mock.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);
            //Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            //Assert- check that i am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mockProc = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(null, mockProc.Object);
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //Assert - check that the order hasn't been passed on to the processor, means that ProcessorOrder was never invoked
            mockProc.Verify(mock=> mock.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once);
            //Assert - check that the method is returning the default view
            Assert.AreEqual("Completed", result.ViewName);
            //Assert- check that i am passing an invalid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
