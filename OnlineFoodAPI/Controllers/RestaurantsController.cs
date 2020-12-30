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
            return db.Restaurant.ToList(); 
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
            foreach(var item in temprestaurants)
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
        public IHttpActionResult PutRestaurant(Restaurant restaurant) //sends in object in body
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(restaurant.id))
                {
                    return NotFound();
                }
                else
                {
                    //TODO: Osäker på hur jag ska komma in här via unit test...
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
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
            foreach(var item in restlist)
            {
                db.FavoritesRestaurants.Remove(item);
            }

            db.Restaurant.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
        }


        private bool RestaurantExists(int id)
        {
            return db.Restaurant.Count(e => e.id == id) > 0;
        }
    }
}