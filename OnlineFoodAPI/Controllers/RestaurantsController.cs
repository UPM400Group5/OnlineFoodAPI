using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OnlineFoodAPI;
using OnlineFoodAPI.Models;

namespace OnlineFoodAPI.Controllers
{
    public class RestaurantsController : ApiController
    {
        private DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel();

        [HttpGet]
        [Route("restaurant/all")]
        public List<Restaurant> GetRestaurant()
        {
            List<Restaurant> restaurants = db.Restaurant.ToList();
            foreach(var item in restaurants)
            {
                item.Dishes = null;
                item.User = null;
                item.FavoritesRestaurants = null;
                /* item.Dishes = db.Dishes.Where(e => e.Restaurant_id == item.id).ToList();
                List<Ingredient> listingtemp = db.Ingredient.ToList();
                foreach (var item2 in item.Dishes)
                {
                    List<Ingredient> dishingredins = new List<Ingredient>();
                    List<DishesIngredient> dishesinglist = db.DishesIngredient.Where(e => e.Dishes_id == item2.id).ToList();
                    foreach (var item3 in dishesinglist)
                    {
                        List<Ingredient> ing = listingtemp.Where(e => e.id == item3.Ingredient_id).ToList();
                        foreach (var item4 in ing)
                        {
                            item4.DishesIngredient = null;
                            dishingredins.Add(item4);
                        }

                    }
                    item2.Ingredient = dishingredins;

                }
                item.User = null;
                item.FavoritesRestaurants = null;
                foreach (var item2 in item.Dishes)
                {
                    item2.DishesIngredient = null;
                    List<Ingredient> newinglist = new List<Ingredient>();
                    foreach (var item3 in item2.Ingredient)
                    {
                        Ingredient new123 = new Ingredient();
                        new123.name = item3.name;
                        newinglist.Add(new123);
                    }
                    item2.Ingredient = newinglist;
                    item2.User = null;
                    item2.Restaurant = null; 

            }*/

            }


            return restaurants;

            
        }

        [Route("restaurant/specific/{id}")]
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult GetRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }


            restaurant.Dishes = db.Dishes.Where(e => e.Restaurant_id == restaurant.id).ToList();
            List<Ingredient> listingtemp = db.Ingredient.ToList();
            foreach(var item in restaurant.Dishes)
            {
                List<Ingredient> dishingredins = new List<Ingredient>();
                List<DishesIngredient> dishesinglist = db.DishesIngredient.Where(e => e.Dishes_id == item.id).ToList();
                foreach(var item2 in dishesinglist)
                {
                    List<Ingredient> ing = listingtemp.Where(e => e.id == item2.Ingredient_id).ToList();
                    foreach(var item3 in ing)
                    {
                        item3.DishesIngredient = null;
                        dishingredins.Add(item3);   
                    }

                }
                item.Ingredient = dishingredins;

            }
            restaurant.User = null;
            restaurant.FavoritesRestaurants = null;
            foreach(var item in restaurant.Dishes)
            {
                item.DishesIngredient = null;
                List<Ingredient> newinglist = new List<Ingredient>();
                foreach(var item2 in item.Ingredient)
                {
                    Ingredient new123 = new Ingredient();
                    new123.name = item2.name;
                    newinglist.Add(new123);
                }
                item.Ingredient = newinglist;
                item.User = null;
                item.Restaurant = null;
            }
            
            return Ok(restaurant);
        }

        [Route("restaurant/getfavrest/{userid}")]
        // GET: all favourite dishes by sending userid
        public List<Restaurant> GetFavouriteRestaurant(int userid) //get the users from header userid, to see which favourite restaurant the user has. 
        {
            List<Restaurant> temprestaurants = new List<Restaurant>(); //make a list of restaurants
            List<Restaurant> restaurants = new List<Restaurant>(); //the list we are returning
            List<FavoritesRestaurants> FavoriteRestaurants = db.FavoritesRestaurants.Where(uid => uid.User_id == userid).ToList(); //see all favourite restraurants the user has.
            if (FavoriteRestaurants.Count != 0) //see so the list has objects in it
            {
                foreach (var item in FavoriteRestaurants)
                {
                    Restaurant restaurant = new Restaurant();
                    temprestaurants.Add(db.Restaurant.Where(fr => fr.id == item.Restaurant_id).FirstOrDefault()); //add restaurant id 
                }
            }
            foreach (var item in temprestaurants)
            {
                item.User = null; //so that the data isnt recursive
                item.FavoritesRestaurants = null; //so that the data isnt recursive
                List<Dishes> tempdish = db.Dishes.Where(e => e.Restaurant_id == item.id).ToList(); // Adds the restaurant.dishes so frontend can use it directly
                item.Dishes = tempdish;
                restaurants.Add(item); //adds the object to the list ready for return
            }
            return restaurants;
        }

        [HttpPut]
        [Route("restaurant/update")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant(Restaurant restaurantIn) //sends in object in body
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Will be null if not found
            Restaurant UpdatedRestaurant = db.Restaurant.Find(restaurantIn.id);

            if (UpdatedRestaurant == null)
            {
                // If null, restaurant does not exist
                return NotFound();
            }

            // This method updates the same way the old did
            UpdatedRestaurant.name = restaurantIn.name;
            UpdatedRestaurant.city = restaurantIn.city;
            UpdatedRestaurant.phonenumber = restaurantIn.phonenumber;
            UpdatedRestaurant.User = restaurantIn.User;
            UpdatedRestaurant.Dishes = restaurantIn.Dishes;
            UpdatedRestaurant.delivery_price = restaurantIn.delivery_price;
            UpdatedRestaurant.email = restaurantIn.email;

            db.SaveChanges();

            return Ok(restaurantIn);
        }

        [HttpPost]
        [Route("restaurant/new")]
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult PostRestaurant(Restaurant restaurant) //works but should send an affirmative if the action goes through instead of error on postman?
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurant.Add(restaurant);
            db.SaveChanges();

            return Ok("Created restaurant! " + restaurant.name);
        }

        /// <summary>
        /// Get all restaurants from a city
        /// </summary>
        /// <param name="city">Name of city</param>
        /// <returns>Restaurants</returns>
        [HttpGet]
        [Route("restaurant/sort/{city}")]
        public List<Restaurant> GetSortedCity(string city)
        {
            return db.Restaurant.Where(x => x.city.ToLower() == city.ToLower()).ToList();
        }

        /// <summary>
        /// Get list of dishes from a restaurant by id
        /// </summary>
        /// <param name="id">Id of restaurant</param>
        /// <returns>Dishes</returns>
        [HttpGet]
        [Route("restaurant/dishes/{id}")]
        public List<Dishes> GetRestaurantDishes(int id)
        {
            return db.Dishes.Where(x => x.Restaurant_id == id).ToList();
        }

        [HttpDelete]
        [Route("restaurant/{id}")]
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            List<FavoritesRestaurants> restlist = db.FavoritesRestaurants.Where(e => e.Restaurant_id == id).ToList(); //removes each restaurant from db so no foreign keys are left.
            foreach (var item in restlist)
            {
                db.FavoritesRestaurants.Remove(item);
            }

            db.Restaurant.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
        }
    }
}