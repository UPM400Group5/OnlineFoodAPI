using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestAPI.Models;

namespace UnitTestAPI
{
    [TestClass]
    public class RestaurantsTest
    {
        [TestMethod]
        public void AddRestaurant()
        {
            bool succeded = false;
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                Restaurant restaurant = new Restaurant { 
                    name = "Max", 
                    adress = "Storgatan 43", 
                    city = "Trollhättan", 
                    delivery_price = 65, 
                    email = "Max@gmail.com", 
                    phonenumber = "34556352342" 
                };
  
                try
                {
                    db.Restaurant.Add(restaurant);
                    db.SaveChanges();
                    succeded = true; // if database gives no error, it works
                }
                catch { }

                // If true, it works
                Assert.IsTrue(succeded);
           
            }
        }
        [TestMethod]
        public void UpdateRestaurant()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {

            }
        }
        [TestMethod]
        public void DeleteRestaurant()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {

            }
        }
        [TestMethod]
        public void RestaurantSorted()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                string city = "göteborg";
                var result = db.Restaurant.Where(x => x.city.ToLower() == city.ToLower()).ToList();

                // Result should not be null
                Assert.AreNotEqual(null, result);
            }
        }
        [TestMethod]
        public void GetRestaurantDishesByRestaurantId()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                int id = 1;
                var result = db.Dishes.Where(x => x.Restaurant_id == id).ToList();

                Assert.AreNotEqual(null, result);
            }
        }
        [TestMethod]
        public void GetFavoriteRestaurantsByUserId()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                int id = 1;

                List<Restaurant> temprestaurants = new List<Restaurant>();
                List<Restaurant> restaurants = new List<Restaurant>();
                List<FavoritesRestaurants> FavoriteRestaurants = db.FavoritesRestaurants.Where(uid => uid.User_id == id).ToList();
                if (FavoriteRestaurants.Count != 0)
                {
                    foreach (var item in FavoriteRestaurants)
                    {
                        Restaurant restaurant = new Restaurant();
                        temprestaurants.Add(db.Restaurant.Where(fr => fr.id == item.Restaurant_id).FirstOrDefault());
                    }
                }

                foreach (var item in temprestaurants)
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.id = item.id;
                    restaurant.name = item.name;
                    restaurant.adress = item.adress;
                    restaurant.city = item.city;
                    restaurant.delivery_price = item.delivery_price;
                    restaurant.email = item.email;
                    restaurant.phonenumber = item.phonenumber;
                    // restaurant.Dishes= item.Dishes;
                    restaurants.Add(restaurant);
                }

                Assert.IsNotNull(restaurants);
            }
        }

    }
}
