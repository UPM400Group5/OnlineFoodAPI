using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;

namespace UnitTestAPI
{
    [TestClass]
    public class LoginTest
    {
        #region Login Testing
            [TestMethod]
            public void FailedLogin()
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
            public void SuccessfulLogin()
            {
                LoginModel model = new LoginModel();
                model.password = "123456";
                model.username = "potionseller";
         
                LoginController loginController = new LoginController();

                var result = loginController.LoginUser(model);

                // A object is returned if successfull, if not then it failed
                Assert.AreEqual(model.username, result.username);
             }
           
             
            
        #endregion
    }
}
