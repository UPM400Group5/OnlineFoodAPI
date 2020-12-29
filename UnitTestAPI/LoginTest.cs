using NUnit.Framework;
using OnlineFoodAPI.Models;
using System.Linq;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI;

namespace UnitTestAPI
{
    public class LoginTest
    {
        [Test]
        public void FailedLogin_ReturnsNull()
        {
            var controller = new LoginController();

            var login = GetWrongUserLogin();

            var result = controller.LoginUser(login);

            Assert.IsNull(result);
        }

        [Test]
        public void SuccessfulLogin_ReturnsModelNotNull()
        {
            var controller = new LoginController();
            var login = GetRightUserLogin();
            
            var result = controller.LoginUser(login);

            Assert.IsNotNull(result);
        }
        LoginModel GetWrongUserLogin()
        {
            return new LoginModel() {username = "mockeduser", password = "123456" };
        }
        LoginModel GetRightUserLogin()
        {
            return new LoginModel() { username = "potionseller", password = "123456" };
        }
    }
}
