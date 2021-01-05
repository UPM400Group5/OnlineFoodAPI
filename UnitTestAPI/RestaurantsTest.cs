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

        //User user;
        //UsersController userController = new UsersController();

        [SetUp]
        public void Setup()
        {
            try
            {
                restaurant = GetNewMockupRestaurant();
                var result = controller.PostRestaurant(restaurant);
                List<Restaurant> listRestaurant = controller.GetRestaurant();
                var tempRestaurant = listRestaurant[(listRestaurant.Count() - 1)];
                restaurant.id = tempRestaurant.id;
            }
            catch (Exception e)
            {

            }
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                controller.ModelState.Clear();
                var result =
                    controller.DeleteRestaurant(restaurant.id);
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
            controller.DeleteRestaurant(restaurant.id);
            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(actionResult);
        }
        #endregion

        #region Read
        [Test]
        public void GetRestaurauntById_Successfully()
        {
            IHttpActionResult actionResult = controller.GetRestaurant(1);
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
            // Get list
            UsersController controllerUser = new UsersController();
            var newfavrestaurant = controllerUser.AddFavRest(1, 2);
            var list = controller.GetFavouriteRestaurant(1);
            Assert.IsNotNull(list);
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


            restaurant.city = "Malmö";
            IHttpActionResult actionResult = controller.PutRestaurant(restaurant);

            // If ok, it succeded
            Assert.IsInstanceOf<OkNegotiatedContentResult<Restaurant>>(actionResult);
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
            UsersController Usercontroller = new UsersController();
            Usercontroller.AddFavRest(2, restaurant.id);
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
            return new Restaurant() { name = "Alberts Bageri", city = "Malmö", adress = "Drottninggatan 23", delivery_price = 10, email = "Nilssons@yahoo.se", phonenumber = "13123441", id = Int32.MaxValue };
        }
        private User GetNewMockupUser()
        {
            return new User() { username = "UnitTestUser123456", password = "123456", role = "user" };
        }
        #endregion
    }
}
