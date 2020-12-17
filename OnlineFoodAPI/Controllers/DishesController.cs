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
        public List<Dishes> GetDishes()
        {
            List<Dishes> alldishes = db.Dishes.ToList();
            try{
                foreach (var item in alldishes)
                {
                    List<Ingredient> temping = new List<Ingredient>();
                    List<DishesIngredient> ingredientlist = db.DishesIngredient.Where(e => e.Dishes_id == item.id).ToList();
                    foreach (var item2 in ingredientlist)
                    {
                        temping.Add(db.Ingredient.Find(item2.Ingredient_id));
                    }
                    item.Ingredient = temping;
                }
            }
            catch (Exception e)
            {
                
            }
            
            return alldishes;
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
            foreach (var item in dishes.Ingredient)
            {
                Ingredient tempingre = db.Ingredient.Where(e => e.name == item.name).FirstOrDefault();
                item.id = tempingre.id;
            }
            Dishes tempdish1 = new Dishes();
            tempdish1.name = dishes.name;
            tempdish1.price = dishes.price;
            if(dishes.specialprice != null || dishes.specialprice == 0)
            {
                tempdish1.specialprice = dishes.specialprice;
            }
            tempdish1.Restaurant_id = dishes.Restaurant_id;
            

            db.Dishes.Add(tempdish1);
            db.SaveChanges();

            
            Dishes tempdish = db.Dishes.Where(e => e.name == dishes.name).FirstOrDefault();
            dishes.id = tempdish.id;
            foreach (var item in dishes.Ingredient)
            {
                DishesIngredient tempdishing = new DishesIngredient();
                tempdishing.Ingredient_id = item.id;
                tempdishing.Dishes_id = dishes.id;
                List<DishesIngredient> checkifexistlist = db.DishesIngredient.Where(e => e.Dishes_id == tempdishing.Dishes_id).ToList();
                List<int> tempIngredientID = new List<int>();
                bool addtodb = true;
                foreach(var item2 in checkifexistlist)
                {
                    if (tempIngredientID.Any(x=> x.Equals(item2.Ingredient_id)))
                    {
                        addtodb = false; //so that the db doesnt update if there already is an ingrediant that exist to the current dish_id
                    }
                }
                if (addtodb) //continue if there is no overlapping of ingredients. 
                {
                    try
                    {
                        db.DishesIngredient.Add(tempdishing);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Could not save to table dishesingredient | " + e);
                    }
                }

            }
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

            DishesIngredient tempdishing = db.DishesIngredient.Where(e => e.Dishes_id == dishid).FirstOrDefault();
            if(tempdishing != null)
            {
                db.DishesIngredient.Remove(tempdishing);
                db.SaveChanges();
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