using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstracts;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123456")).Returns(true);
            LoginViewModel model = new LoginViewModel {Password = "123456", Username = "admin"};
            AccountController target = new AccountController(mock.Object);


            var result = target.Login(model, "success");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("success", ((RedirectResult) result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123456")).Returns(true);
            LoginViewModel model = new LoginViewModel { Password = "badpassword", Username = "badusername" };
            AccountController target = new AccountController(mock.Object);

            var result = target.Login(model, "bad");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);

        }
    }
}
