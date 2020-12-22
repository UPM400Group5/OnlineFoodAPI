using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineFoodAPI.Models;
using System.Linq;

namespace UnitTestAPI
{
    [TestClass]
    public class LoginTest
    {
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
                Assert.IsNull(result);
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
                Assert.IsNotNull(result);
            }
        }
    }
}
