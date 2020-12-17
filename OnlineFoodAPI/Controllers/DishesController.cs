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
    public class DishesController : ApiController
    {
        private DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel();

        [HttpGet]
        // attribute routing
        [Route("dishes/alldishes")]
        public IQueryable<Dishes> GetDishes()
        {
            return db.Dishes;
        }

        [Route("dishes/getspecificdish/{id}")]
        // GET: specificDish by sending id
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult GetDishes(int id)
        {
            Dishes dishes = db.Dishes.Find(id);
            if (dishes == null)
            {
                return NotFound();
            }

            return Ok(dishes);
        }

        [HttpPut]
        [Route("dishes/updatedish/{id}/{userid}")] //send in a dish aswell with updated information  TODO: STILL HAS TO BE TESTED
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDishes(int id, Dishes dishes, User userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role != "admin")
            {
                return BadRequest("User is not a admin");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dishes.id)
            {
                return BadRequest();
            }
            if(dishes.specialprice != null || dishes.specialprice != 0) //Checks if price is changed after specialprice is
            {
                Dishes dish = db.Dishes.Find(dishes.id);
                if(dishes.price != dish.price)
                {
                    return BadRequest("Remove specialprice before changing normal price");
                }
            }

            db.Entry(dishes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishesExists(id))
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
        [Route("dishes/gettotalpriceofbasket")]  //TODO - TEST if it works. 
        public string GetTotalPriceOfBasket(List<Dishes> Ordereddishes)
        {
            int totalprice = 0;
            foreach (var item in Ordereddishes)
            {
                if (item.price != 0 && item.specialprice == null)
                {
                    return "something isn't quite right, ERROR: price isnt set";

                }
                if (item.specialprice == null)
                {
                    totalprice += item.price;
                }
                else if (item.specialprice < item.price && item.specialprice != null)
                {
                    totalprice += item.specialprice.GetValueOrDefault();
                }

            }
            return totalprice.ToString();
        }

        [HttpPost]
        [Route("dishes/NewDish/{userid}")] //Todo: maybe check if the dish belongs to the restaurant that wants to change
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult PostDishes(Dishes dishes, int userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role != "admin")
            {
                return BadRequest("User is not a admin");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dishes.Add(dishes);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dishes.id }, dishes);
        }

        [Route("dishes/delete/{dishid}/{userid}")]  //TODO test
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult DeleteDishes(int dishid, int userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role != "admin")
            {
                return BadRequest("User is not a admin");
            }
            Dishes dishes = db.Dishes.Find(dishid);
            if (dishes == null)
            {
                return NotFound();
            }

            db.Dishes.Remove(dishes);
            db.SaveChanges();

            return Ok(dishes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DishesExists(int id)
        {
            return db.Dishes.Count(e => e.id == id) > 0;
        }
    }
}