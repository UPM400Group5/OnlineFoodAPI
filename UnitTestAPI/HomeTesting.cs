using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineFoodAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAPI
{
    [TestClass]
    public class HomeTesting
    {
        [TestMethod]
        public void TestHomeAsync()
        {
            var controller = new HomeController();
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(System.Web.Mvc.ViewResult));
        }
    }
}
