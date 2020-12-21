using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;
using System.Linq;

namespace UnitTestAPI
{
    [TestClass]
    public class LoginTest
    {
        #region Login Testing
            [TestMethod]
            public void FailedLogin()
            {
                using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel()) 
                {
                    LoginModel model = new LoginModel();
                    model.password = "123456";
                    model.username = "Admin";

                    var result = db.User.Where(x => x.username == model.username && x.password == model.password).FirstOrDefault();

                    // Should be null if failed
                    Assert.AreEqual(null, result);
                } 
            }

            [TestMethod]
            public void SuccessfulLogin()
            {
                using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
                {
                    LoginModel model = new LoginModel();
                    model.password = "123456";
                    model.username = "potionseller";

                    var result = db.User.Where(x => x.username == model.username && x.password == model.password).FirstOrDefault();

                    // An object of user is returned if successfull, if not then it failed
                    Assert.AreEqual(model.username, result.username);
                }
            }
           
             
            
        #endregion
    }
}
