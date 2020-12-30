using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI;
using OnlineFoodAPI.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;

namespace UnitTestAPI
{
    public class RestaurantsTest
    {
        Restaurant restaurant;
        RestaurantsController controller = new RestaurantsController();

        User user;
        UsersController userController = new UsersController();

        [SetUp]
        public void Setup()
        {
            restaurant = GetNewMockupRestaurant();
            var result = controller.PostRestaurant(restaurant);
            List<Restaurant> listRestaurant = controller.GetRestaurant();
            var tempRestaurant = listRestaurant[(listRestaurant.Count() - 1)];
            restaurant.id = tempRestaurant.id;

            user = GetNewMockupUser();
            var userResult = userController.PostUser(user);
            List<User> listuser = userController.GetAllUsers(2).ToList();
            var tempuser = listuser[(listuser.Count() - 1)];
            user.id = tempuser.id;
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                controller.ModelState.Clear();
                var result =
                    controller.DeleteRestaurant(restaurant.id);

                controller.ModelState.Clear();
                var userResult =
                    userController.DeleteUser(user.id);
            }
            catch (Exception e)
            {

            }
        }

        #region Create
        [Test]
        public void PostRestaurant_BadRequest()
        {
            Restaurant temp = GetNoneExistingRestaurant();
            controller.ModelState.AddModelError("test", "test");

            IHttpActionResult actionResult = controller.PostRestaurant(temp);
            Assert.IsInstanceOf<InvalidModelStateResult>(actionResult);
        }

        [Test]
        public void PostRestaurant_Successfully()
        {
            IHttpActionResult actionResult = controller.PostRestaurant(restaurant);
            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(actionResult);
        }
        #endregion

        #region Read
        [Test]
        public void GetRestaurauntById_Successfully()
        {
            IHttpActionResult actionResult = controller.GetRestaurant(restaurant.id);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Restaurant>>(actionResult);
        }

        [Test]
        public void GetRestaurauntById_NotFound()
        {
            IHttpActionResult actionResult = controller.GetRestaurant(Int32.MaxValue);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void GetRestaurantList_Successfully()
        {
            var list = controller.GetRestaurant();
            Assert.IsTrue(list != null);
        }

        [Test]
        public void GetFavouriteRestaurant_Successfully()
        {
            // Add favorite first
            userController.AddFavRest(user.id, restaurant.id);

            // Get list
            var list = controller.GetFavouriteRestaurant(user.id);
            Assert.IsTrue(list != null);
        }

        [Test]
        public void GetSortedCities_Successfully()
        {
            string city = "Göteborg";
            var list = controller.GetSortedCity("Göteborg");

            int total = list.Count;

            // Count each by city
            int byCity = (list.Where(x => x.city.ToLower() == city.ToLower())).ToList().Count;

            Assert.AreEqual(byCity, total);
        }

        [Test]
        public void GetRestaurantDishes_Successfully()
        {
            //TODO: hardcoded
            var list = controller.GetRestaurantDishes(1);
            Assert.IsNotNull(list);
        }
        #endregion

        #region Update
        [Test]
        public void UpdateRestaurant_Successfully()
        {
            // New name
            restaurant.name = "Alberts";
            IHttpActionResult actionResult = controller.PutRestaurant(restaurant);

            var message = actionResult as StatusCodeResult;

            // If ok, it succeded
            Assert.AreEqual("ok",message.StatusCode.ToString().ToLower());
        }
        [Test]
        public void UpdateRestaurant_NotFound()
        {
            Restaurant temp = GetNoneExistingRestaurant();
            IHttpActionResult actionResult = controller.PutRestaurant(temp);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        [Test]
        public void UpdateRestaurant_BadRequest()
        {
            restaurant.city = "Malmö";
            controller.ModelState.AddModelError("test", "test");

            IHttpActionResult actionResult = controller.PutRestaurant(restaurant);
            Assert.IsInstanceOf<InvalidModelStateResult>(actionResult);
        }
        #endregion

        #region Delete
        [Test]
        public void DeleteRestaurant_Successfully()
        {
            IHttpActionResult actionResult = controller.DeleteRestaurant(restaurant.id);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Restaurant>>(actionResult);
        }
        [Test]
        public void DeleteRestaurant_NotFound()
        {
            IHttpActionResult actionResult = controller.DeleteRestaurant(Int32.MaxValue);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        #endregion

        #region Mockup data
        private Restaurant GetNewMockupRestaurant()
        {
            return new Restaurant() { name = "Nilssons Pizzeria", city = "Trollhättan", adress = "Hamngatan 23", delivery_price = 10, email = "Nilssons@yahoo.se", phonenumber = "13123441" };
        }
        private Restaurant GetNoneExistingRestaurant()
        {
            return new Restaurant() { name = "Nilssons Pizzeria", city = "Trollhättan", adress = "Hamngatan 23", delivery_price = 10, email = "Nilssons@yahoo.se", phonenumber = "13123441", id = Int32.MaxValue };
        }
        private User GetNewMockupUser()
        {
            return new User() { username = "UnitTestUser123456", password = "123456", role = "user" };
        }
        #endregion
    }
}
