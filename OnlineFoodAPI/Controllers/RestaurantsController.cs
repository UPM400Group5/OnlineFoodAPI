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
        [Route("restaurants/all")]
        public IQueryable<Restaurant> GetRestaurant()
        {
            return db.Restaurant;
        }

        [Route("restaurants/specific/{id}")]
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult GetRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }



        [Route("restaurant/getfavrest/{userid}")]
        // GET: all favourite dishes by sending userid
        public List<Restaurant> GetFavouriteRestaurant(int userid)
        {
            List<Restaurant> temprestaurants = new List<Restaurant>();
            List<Restaurant> restaurants = new List<Restaurant>();
            List<FavoritesRestaurants> FavoriteRestaurants = db.FavoritesRestaurants.Where(uid => uid.User_id == userid).ToList();
            if (FavoriteRestaurants.Count != 0)
            {
                foreach (var item in FavoriteRestaurants)
                {
                    Restaurant restaurant = new Restaurant();
                    temprestaurants.Add(db.Restaurant.Where(fr => fr.id == item.Restaurant_id).FirstOrDefault());
                }
            }
            foreach(var item in temprestaurants)
            {
                Restaurant restaurant = new Restaurant();
                restaurant.id = item.id;
                restaurant.name = item.name;
                restaurant.adress = item.adress;
                restaurant.city = item.city;
                restaurant.delivery_price = item.delivery_price;
                restaurant.Dishes= item.Dishes;
                restaurants.Add(restaurant);


            }
            return restaurants;
        }

        // PUT: api/Restaurants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant(int id, Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant.id)
            {
                return BadRequest();
            }

            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
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

            return CreatedAtRoute("DefaultApi", new { id = restaurant.id }, restaurant);
        }


        // DELETE: api/Restaurants/5
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            db.Restaurant.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantExists(int id)
        {
            return db.Restaurant.Count(e => e.id == id) > 0;
        }
    }
}