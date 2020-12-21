using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineFoodAPI.Controllers;
using System.Linq;
using System.Collections.Generic;

namespace UnitTestAPI
{
    [TestClass]
    public class RestaurantsTest
    {
        [TestMethod]
        public void AddRestaurant()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel()) 
            {

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
        public void RestaurantDishes()
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                int id = 1;
                var result =  db.Dishes.Where(x => x.Restaurant_id == id).ToList();

                Assert.AreNotEqual(null, result);
            }
        }
        [TestMethod]
        public void FavorieRestaurants() 
        {
            using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
            {
                //TODO: fixa modellerna
                List<Restaurant> temprestaurants = new List<Restaurant>();
                List<Restaurant> restaurants = new List<Restaurant>();
               // List<OnlineFoodAPI.Models.FavoritesRestaurants> FavoriteRestaurants = db.FavoritesRestaurants.Where(uid => uid.User_id == userid).ToList();
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
            }
        }

    }
}
