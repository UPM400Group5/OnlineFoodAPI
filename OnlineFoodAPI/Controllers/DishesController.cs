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
            try
            {
                foreach (var item in alldishes)
                {
                    List<Ingredient> temping = new List<Ingredient>();
                    List<DishesIngredient> ingredientlist = db.DishesIngredient.Where(e => e.Dishes_id == item.id).ToList();
                    foreach (var item2 in ingredientlist)
                    {
                        Ingredient ingridienttemp = db.Ingredient.Find(item2.Ingredient_id);
                        ingridienttemp.Dishes = null;
                        ingridienttemp.DishesIngredient = null;
                        temping.Add(ingridienttemp);
                    }
                    item.Ingredient = temping;
                    item.DishesIngredient = null;  

                }
            }
            catch (Exception e){}
            return alldishes;
        }

        [HttpPut]
        [Route("dishes/update/{id}/{userid}")]
        public string PutDishess(int id, int userid, Dishes dishes)
        {
            User checkifadmin = db.User.Find(userid); //check if the user should be able to continue
            if (checkifadmin.role != "admin")
            {
                return "User is not a admin";
            } 
            if (id != dishes.id)
            {
                return "id doesnt extist";
            }
            if (dishes.specialprice == null || dishes.specialprice == 0) //Checks if price is changed after specialprice is
            {
                Dishes dish = db.Dishes.Find(dishes.id);
                if (dishes.price != dish.price)
                {
                    return "Remove specialprice before changing normal price";
                }
            }
            try
            {
                DishesIngredient tempdishing = db.DishesIngredient.Where(e => e.Dishes_id == dishes.id).FirstOrDefault();
                db.DishesIngredient.Attach(tempdishing);
                db.Entry(tempdishing).State = EntityState.Deleted;
                db.SaveChanges();
                
            }
            catch{}
            try
            {
                foreach (var item in dishes.Ingredient)
                {
                    Ingredient temping = new Ingredient();
                    Ingredient testing = db.Ingredient.Where(e => e.name == item.name).FirstOrDefault();
                    Ingredient ing_id = new Ingredient();
                    if (testing == null)
                    {
                        temping.name = item.name;
                        db.Ingredient.Add(temping);
                        ing_id = db.Ingredient.Where(e => e.name == temping.name).FirstOrDefault();
                        db.SaveChanges();
                    }
                    else
                    {
                        ing_id = testing;
                    }
                    DishesIngredient tempdishtoaddtotable = new DishesIngredient();
                    tempdishtoaddtotable.Dishes_id = dishes.id;
                    tempdishtoaddtotable.Ingredient_id = ing_id.id;
                    db.DishesIngredient.Add(tempdishtoaddtotable);
                    db.SaveChanges();
                }
            }
            catch { }
            try
            {
                var activityinDb = db.Dishes.Find(dishes.id);
                if(activityinDb == null)
                {
                    db.Dishes.Add(dishes);
                    db.SaveChanges();
                }
                activityinDb.name  = dishes.name;
                activityinDb.price = dishes.price;
                //activityinDb.Ingredient = temping; //NEVER UPDATE THIS DB WITH INGREDIENT
                activityinDb.Restaurant_id = dishes.Restaurant_id;
                activityinDb.Restaurant = db.Restaurant.Find(dishes.Restaurant_id);
                activityinDb.specialprice = dishes.specialprice;
                db.Entry(activityinDb).State = EntityState.Modified;  //See if ingredient already exists
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishesExists(id)){return "dish doesnt exist";}
                else{throw;}
            } return "updated";
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
            try
            {
                List<Ingredient> temping = new List<Ingredient>();
                List<DishesIngredient> ingredientlist = db.DishesIngredient.Where(e => e.Dishes_id == dishes.id).ToList();
                foreach (var item in ingredientlist)
                {
                    Ingredient ingridienttemp = db.Ingredient.Find(item.Ingredient_id);
                    ingridienttemp.Dishes = null;
                    ingridienttemp.DishesIngredient = null;
                    temping.Add(ingridienttemp);
                }
                dishes.Ingredient = temping;
                dishes.DishesIngredient = null; 
            }
            catch (Exception e) { }
            return Ok(dishes);
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
            if (dishes.specialprice != null || dishes.specialprice == 0)
            {
                tempdish1.specialprice = dishes.specialprice;
            }
            tempdish1.Restaurant_id = dishes.Restaurant_id;

            db.Dishes.Add(tempdish1); //First add dish to dishes
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
                foreach (var item2 in checkifexistlist)
                {
                    if (tempIngredientID.Any(x => x.Equals(item2.Ingredient_id)))
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

        [HttpDelete]
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
            if (tempdishing != null)
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