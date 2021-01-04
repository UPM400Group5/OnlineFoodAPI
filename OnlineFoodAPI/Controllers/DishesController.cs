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
                foreach (var item in alldishes) //make sure ingredient exist as an object.
                {
                    List<Ingredient> temping = new List<Ingredient>();
                    List<DishesIngredient> ingredientlist = db.DishesIngredient.Where(e => e.Dishes_id == item.id).ToList(); //find every dish that has an id connected to each dish from dbo.DishesIngredient
                    foreach (var item2 in ingredientlist)
                    {
                        Ingredient ingridienttemp = db.Ingredient.Find(item2.Ingredient_id); //find each ingredient to make an object out of it.
                        ingridienttemp.Dishes = null; //so the data isnt recursive
                        ingridienttemp.DishesIngredient = null; //so the data isnt recursive
                        temping.Add(ingridienttemp); //add ingredient to list 
                    }
                    item.Ingredient = temping; //specify that each ingredient associated to the Dish(item) is an object.
                    item.DishesIngredient = null;  //so the data isnt recursive  

                }
            }
            catch (Exception e) { }
            return alldishes;
        }

        [HttpPut]
        [Route("dishes/update/{id}/{userid}")]
        public string PutDishess(int id, int userid, Dishes dishes)
        {
            User checkifadmin = db.User.Find(userid); //check if the user should be able to continue
            if (checkifadmin.role != "admin")
            {
                return "User is not a admin"; //returns error if false.
            }
            if (id != dishes.id)
            {
                return "id doesnt exist";
            }
            if (dishes.specialprice == null || dishes.specialprice == 0) //Checks if price is changed after specialprice is
            {
                Dishes dish = db.Dishes.Find(dishes.id);
                if (dishes.price != dish.price)
                {
                    return "Remove specialprice before changing normal price"; //hard to test
                }
            }
            List<DishesIngredient> tempdishing = db.DishesIngredient.Where(e => e.Dishes_id == id).ToList(); //add all dishesingredient from dbo.dishesingredient that has dish_id as dishid sent in header

            if (dishes.Ingredient == null)
            {
                return "There was no ingredient attached";
            }
            else
            {
                RemoveIngredients(tempdishing);
                Dishes tempdish = db.Dishes.Find(dishes.id);
                tempdish.Ingredient = null;
                db.SaveChanges();
            }
            try
            {
                foreach (var item in dishes.Ingredient) //now we add what we deleted, just renewed to the current models ingredients.
                {
                    Ingredient temping = new Ingredient();
                    Ingredient testing = db.Ingredient.Where(e => e.name == item.name).FirstOrDefault(); //search ingredients of the dishes ingredient name, only thing user sends.
                    Ingredient ing_id = new Ingredient();
                    if (testing == null) //if it doesnt find we have to add the ingredient.
                    {
                        temping.name = item.name;
                        db.Ingredient.Add(temping);
                        ing_id = db.Ingredient.Where(e => e.name == temping.name).FirstOrDefault();
                        db.SaveChanges();
                    }
                    else //else the id is the one we found.
                    {
                        ing_id = testing;
                    }
                    DishesIngredient tempdishtoaddtotable = new DishesIngredient(); //Create new dishesingredient 
                    tempdishtoaddtotable.Dishes_id = dishes.id;
                    tempdishtoaddtotable.Ingredient_id = ing_id.id;
                    db.DishesIngredient.Add(tempdishtoaddtotable); // Add to dbo.dishesingredient with the new dish_id and Ingredient_id
                    db.SaveChanges();
                }
            }
            catch (Exception e) { }
            //Now we have to update the object.

            var activityinDb = db.Dishes.Find(dishes.id);
            if (activityinDb == null)
            {
                db.Dishes.Add(dishes);
                db.SaveChanges();
            }
            activityinDb.name = dishes.name;
            activityinDb.price = dishes.price;
            //activityinDb.Ingredient = temping; //NEVER UPDATE THIS DB WITH INGREDIENT [saving this for error that can be found later]
            activityinDb.Restaurant_id = dishes.Restaurant_id;
            activityinDb.Restaurant = db.Restaurant.Find(dishes.Restaurant_id); //restaurang is an object so we search for the id in dbo.restaurang
            activityinDb.specialprice = dishes.specialprice;
            db.Entry(activityinDb).State = EntityState.Modified;  //Db knows what to update.


            db.SaveChanges();

            return "updated";
        }
        [Route("dishes/getspecificdish/{id}")]
        // GET: specificDish by sending id
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult GetDishes(int id)
        {
            Dishes dishes = db.Dishes.Find(id); //make an object of the dishid sent in.
            if (dishes == null)
            {
                return NotFound(); //return notfound if the dish_Id is wrong
            }

            List<Ingredient> temping = new List<Ingredient>();
            List<DishesIngredient> ingredientlist = db.DishesIngredient.Where(e => e.Dishes_id == dishes.id).ToList(); //adds ingredient that are associated by dbo.DishesIngredient
            foreach (var item in ingredientlist)
            {
                Ingredient ingridienttemp = db.Ingredient.Find(item.Ingredient_id);
                ingridienttemp.Dishes = null; //make them null to not send it recursive data
                ingridienttemp.DishesIngredient = null; //make them null to not send it recursive data
                temping.Add(ingridienttemp); //Add ingredient tempinglist associated to the dish.ingredient.
            }
            dishes.Ingredient = temping;
            dishes.DishesIngredient = null;  //make them null to not send it recursive data

            return Ok(dishes);
        }

        [HttpPost]
        [Route("dishes/gettotalpriceofbasket")]  //TODO - TEST if it works. 
        public string GetTotalPriceOfBasket(List<Dishes> Ordereddishes)
        {
            int totalprice = 0;
            foreach (var item in Ordereddishes)
            {
                if (item.price == 0 && item.specialprice == null)
                {
                    return "something isn't quite right, ERROR: price isnt set";
                }
                if (item.specialprice == null || item.specialprice == 0)
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
            User checkifadmin = db.User.Find(userid); //checks if the user is admin from userid in header of api
            if (checkifadmin.role != "admin")
            {
                return BadRequest("User is not a admin"); //return if the user isnt admin
            }
            if (!ModelState.IsValid) //check if the model is valid
            {
                return BadRequest(ModelState);
            }
            foreach (var item in dishes.Ingredient) //have to specifiy id of each ingredient sent in with the model
            {

                Ingredient tempingre = db.Ingredient.Where(e => e.name == item.name).FirstOrDefault(); //check the ingredient in dbo.ingredient, since user only sends in ingredientName
                if (tempingre == null)
                {
                    db.Ingredient.Add(item);
                    db.SaveChanges();
                    tempingre = db.Ingredient.Where(e => e.name == item.name).FirstOrDefault();
                }

                item.id = tempingre.id; //taken id from above is set to the ingredient object(item).



            }
            Dishes tempdish1 = new Dishes(); //make a temporary dish
            tempdish1.name = dishes.name;
            tempdish1.price = dishes.price;
            if (dishes.specialprice != null || dishes.specialprice == 0) //make sure the specialprice is the one to add
            {
                tempdish1.specialprice = dishes.specialprice;
            }
            tempdish1.Restaurant_id = dishes.Restaurant_id;

            db.Dishes.Add(tempdish1); //First add dish to dishes 
            db.SaveChanges();

            List<Dishes> tempdishlist = db.Dishes.Where(e => e.name == dishes.name).ToList();
            Dishes tempdish = tempdishlist[tempdishlist.Count() - 1]; //Search for the id of the dish we just saved in DB, since we didnt have it before we created it.
            dishes.id = tempdish.id;
            foreach (var item in dishes.Ingredient) //saving each dish_id and ingredient_id to dbo.DishesIngredient
            {
                DishesIngredient tempdishing = new DishesIngredient(); //create temporary object from dishesingredient model
                tempdishing.Ingredient_id = item.id;
                tempdishing.Dishes_id = dishes.id;
                List<DishesIngredient> checkifexistlist = db.DishesIngredient.Where(e => e.Dishes_id == tempdishing.Dishes_id).ToList(); //searches dbo.dishesingredient to see if it already exists.
                List<int> tempIngredientID = new List<int>(); //temporary list of ingredientIDs
                foreach (var intitem in checkifexistlist)
                {
                    tempIngredientID.Add(item.id);
                }
                bool addtodb = true; //bool to see if we should continue and that there are no overlapping ingredients.
                foreach (var item2 in checkifexistlist)
                {
                    if (tempIngredientID.Any(x => x.Equals(item2.Ingredient_id))) //lambda expression to see if any ingredient overlaps. 
                    {
                        addtodb = false; //so that the db doesnt update if there already is an ingrediant that exist to the current dish_id
                    }
                }
                if (addtodb) //continue if there is no overlapping of ingredients. 
                {

                    db.DishesIngredient.Add(tempdishing); //save the dishingredient to dbo.dishingredient
                    db.SaveChanges();


                }
            }
            return Ok("Succesfully added new dish");
        }

        [HttpDelete]
        [Route("dishes/delete/{dishid}/{userid}")]
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult DeleteDishes(int dishid, int userid)
        {
            User checkifadmin = db.User.Find(userid); //checks if user is admin 
            if (checkifadmin.role != "admin")
            {
                return BadRequest("User is not a admin"); //returns error that UserID has to be admin
            }
            Dishes dishes = db.Dishes.Find(dishid); //searches for the dish with the dishid sent in header of api
            if (dishes == null)
            {
                return NotFound(); //returns not found
            }

            List<DishesIngredient> tempdishing = db.DishesIngredient.Where(e => e.Dishes_id == dishid).ToList(); //add all dishesingredient from dbo.dishesingredient that has dish_id as dishid sent in header
            if (tempdishing.Count != 0)
            {
                foreach (DishesIngredient item in tempdishing) //each dishingredient in tempdishing list
                {
                    db.DishesIngredient.Remove(item); //removes all rows where item(dishingredient with dish_id == dishid) exists.

                }
            }

            try
            {
                db.Dishes.Remove(dishes); //then we can remove the dish from dbo.dishes since there is not conflict of foreign key
                db.SaveChanges();
            }
            catch (Exception e) { }

            return Ok(dishes); //return the deleted dish
        }
        public void RemoveIngredients(List<DishesIngredient> tempdishing)
        {
            if (tempdishing.Count != 0)
            {
                foreach (DishesIngredient item in tempdishing) //each dishingredient in tempdishing list
                {
                    db.DishesIngredient.Remove(item); //removes all rows where item(dishingredient with dish_id == dishid) exists.

                }


            }


        }

    }


}