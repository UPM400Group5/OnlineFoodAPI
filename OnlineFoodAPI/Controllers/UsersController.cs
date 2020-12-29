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
    public class UsersController : ApiController
    {
        private DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel();

        // get: api/Users
        [HttpGet]
        [Route("Users/All/{id}")]
        /// <summary>
        /// Get all data from users, send in current userID. Should only be allowed as admin
        /// </summary>
        [ResponseType(typeof(List<UserModel>))]
        public List<User> GetAllUsers(int id)
        {
            // Check user by id, then check role and make it lowercase
            if (db.User.Find(id).role.ToLower() == "admin") 
            {
                return db.User.ToList(); //return all user if the user is admin
            }
            return null;
        }

         [Route("Users/{id}")]
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id); //find user from id in header of api
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("Users/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user) //id is from header of api, user is sent from body
        {
            //user = db.User.Find(id); //find user from id
            if (!ModelState.IsValid) //check if the model is valid
            {
                return BadRequest(ModelState);
            }

            if (id != user.id) //see if sent id is the same as user
            {
                return BadRequest("Cant find user");
            }

            // If password length is less than 6, dont continue
            if (user.password.Length < 6) 
            {
                return BadRequest("Password length is less than 6");
            }

            db.Entry(user).State = EntityState.Modified; //tell db to modify the user

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!UserExists(id)) //if user doesnt exist with the id
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
        [Route("Users")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user) //new user
        {
            if (!ModelState.IsValid) //check if the model is valid
            {
                return BadRequest(ModelState);
            }

            if (user.password.Length < 6)  //check if password length is less than 6
            {
                return BadRequest("Password length is less than 6");
            }

            db.User.Add(user); //add user to db
            db.SaveChanges();

            return Ok(user);
        }

        #region FAVOURITE RESTAURANTS
        [HttpGet]
        [Route("Users/addfavrest/{userid}/{restid}")]
        public string AddFavRest(int userid, int restid) //Ad, with GET, Favourite restaurant to db. Since user sho uld only press an icon to add.
        {
            User user = db.User.Find(userid); //find user with logged in userid sent through header of api
            var Restaurant = db.Restaurant.Find(restid); //find restaurant with restaurantID sent through header of api
            FavoritesRestaurants favoritesRestaurant = new FavoritesRestaurants(); //specify the new favouriterestaurant with an object to send to dbo.FavouriteRestaurant
            favoritesRestaurant.Restaurant_id = Restaurant.id;
            favoritesRestaurant.User_id = user.id;
            try
            {
                db.FavoritesRestaurants.Add(favoritesRestaurant); //add to db
                db.SaveChanges();
                return "Success";
            }
            catch(Exception e)
            {
                return "exception " + e.Message;
            }
        }

        [HttpDelete]
        [Route("Users/Removefavrest/{userid}/{restid}")]   //Removes favrestauarant - Tested and works
        public string RemoveFavRest(int userid, int restid) //only from header of api since the user should only press a button to 'unsubscribe'
        {
            User user = db.User.Find(userid); //find user in dbo.User with userid from header of api
            var Restaurant = db.Restaurant.Find(restid); //find restaurant from dbo.restaurants with restid from header of api
            if (Restaurant == null) //if restaurant cant be found
            {
                return "could not find the restaurant";
            }
            FavoritesRestaurants favoritesRestaurant = new FavoritesRestaurants();  //make an object out of the data we found
            favoritesRestaurant.Restaurant_id = Restaurant.id; 
            favoritesRestaurant.User_id = user.id;
            try
            {
                db.FavoritesRestaurants.Attach(favoritesRestaurant); //telling db what kind of object to delete
                db.Entry(favoritesRestaurant).State = EntityState.Deleted;  //db deletes it
                db.SaveChanges(); //save db
                return "Success";
            }
            catch (Exception e)
            {
                return "exception " + e.Message;
            }
        }
        #endregion


        [HttpDelete]
        [Route("Users/{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id) //delete user with only an id from header of api
        {
            User user = db.User.Find(id); //find the user from dbo.User using the id from header of api
            if (user == null) //if the user cant be found return notfound();
            {
                return NotFound();
            }
            List<FavoritesRestaurants> templist = db.FavoritesRestaurants.Where(e => e.User_id == user.id).ToList(); //have to remove foreign key restraints in dbo.FavouriteRestaurant
            foreach(var item in templist)
            {
                db.FavoritesRestaurants.Remove(item); //remove each object connected to the user
            }

            db.User.Remove(user); //no foreignkey restraints, clear to delete the user.
            db.SaveChanges();

            return Ok(user);
        }


        private bool UserExists(int id)
        {
            return db.User.Count(e => e.id == id) > 0;
        }   
    }
}