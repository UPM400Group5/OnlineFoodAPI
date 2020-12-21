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
        public List<UserModel> GetAllUsers(int id)
        {
            // Check user by id, then check role and make it lowercase
            if (db.User.Find(id).role.ToLower() == "admin") 
            {
                // db.users did not work. I had to make a 
                List<UserModel> userList = new List<UserModel>();
                var users = db.User;

                foreach (var item in users)
                {
                    UserModel model = new UserModel(item);
                    userList.Add(model);
                }
                return userList;
            }
            return null;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            user = db.User.Find(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.id)
            {
                return BadRequest();
            }

            // If password length is less than 6, dont continue
            if (user.password.Length < 6) 
            {
                //TODO: annat meddelande?
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]

        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user.password.Length < 6)
            {
                //TODO: annat meddelande?
                return BadRequest();
            }

            db.User.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.id }, user);
        }

        #region FAVOURITE RESTAURANTS
        [HttpGet]
        [Route("Users/addfavrest/{userid}/{restid}")]
        public string AddFavRest(int userid, int restid)
        {
            User user = db.User.Find(userid);
            var Restaurant = db.Restaurant.Find(restid);
            FavoritesRestaurants favoritesRestaurant = new FavoritesRestaurants();
            favoritesRestaurant.Restaurant_id = Restaurant.id;
            favoritesRestaurant.User_id = user.id;
            try
            {
                db.FavoritesRestaurants.Add(favoritesRestaurant);
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
        public string RemoveFavRest(int userid, int restid)
        {
            User user = db.User.Find(userid);
            var Restaurant = db.Restaurant.Find(restid);
            if (Restaurant == null)
            {
                return "could not find the restaurant";
            }
            FavoritesRestaurants favoritesRestaurant = new FavoritesRestaurants();
            favoritesRestaurant.Restaurant_id = Restaurant.id;
            favoritesRestaurant.User_id = user.id;
            try
            {
                db.FavoritesRestaurants.Attach(favoritesRestaurant);
                db.Entry(favoritesRestaurant).State = EntityState.Deleted;
                db.SaveChanges();
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
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            List<FavoritesRestaurants> templist = db.FavoritesRestaurants.Where(e => e.User_id == user.id).ToList();
            foreach(var item in templist)
            {
                db.FavoritesRestaurants.Remove(item);
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.id == id) > 0;
        }

        
    }
}