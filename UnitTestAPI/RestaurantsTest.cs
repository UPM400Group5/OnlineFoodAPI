using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineFoodAPI.Controllers;

namespace UnitTestAPI
{
    [TestClass]
    public class RestaurantsTest
    {
        private RestaurantsController handler = new RestaurantsController();

        [TestMethod]
        public void AddRestaurant()
        {

        }
        [TestMethod]
        public void UpdateRestaurant()
        {

        }
        [TestMethod]
        public void DeleteRestaurant()
        {

        }
        [TestMethod]
        public void RestaurantSorted()
        {

            RestaurantsController restaurantsController = new RestaurantsController();

            var result = restaurantsController.GetSortedCity("göteborg");

            // A object is returned if successfull, if not then it failed
            Assert.AreNotEqual(null, result);
        }
        public void RestaurantDishes()
        {

        }

    }
}
