using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;

namespace UnitTestAPI
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFailedLogin()
        {
            LoginModel model = new LoginModel();
            model.password = "12345";
            model.username = "Admin";

            LoginController loginController = new LoginController();

            var result = loginController.LoginUser(model);

            // Should be null if failed
            Assert.AreEqual(null, result);
        }
        [TestMethod]
        public void TestSuccessful()
        {
            LoginModel model = new LoginModel();
            model.password = "12345";
            model.username = "Admin";

            LoginController loginController = new LoginController();

            var result = loginController.LoginUser(model);

            // A object is returned if successfull, if not then it failed
            Assert.AreEqual(result.username, model.username);
        }
    }
}
