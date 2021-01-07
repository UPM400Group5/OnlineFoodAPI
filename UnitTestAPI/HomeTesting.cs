using NUnit.Framework;
using OnlineFoodAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAPI
{
    public class HomeTesting
    {
        [Test]
        public void TestHomeAsync()
        {
            var controller = new HomeController();
            var result = controller.Index();
            Assert.IsInstanceOf< System.Web.Mvc.ViewResult> (result);

        }
    }
}
