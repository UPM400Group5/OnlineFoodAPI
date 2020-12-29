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

namespace OnlineFoodAPI.Controllers
{
    public class IngredientsController : ApiController
    {
        private DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel();

        [HttpGet]
        [Route("ingredient/getall")]
        public IQueryable<Ingredient> GetIngredient()
        {
            return db.Ingredient; //return what the dbo.ingredient has
        }

        [Route("ingredient/getspecific/{id}")]
        [ResponseType(typeof(Ingredient))]
        public IHttpActionResult GetIngredient(int id)
        {
            Ingredient ingredient = db.Ingredient.Find(id); //find specific id from header of api
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPut]
        [Route("ingredient/update/{userid}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIngredient( Ingredient ingredient, int userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role.ToLower() != "admin") //check if the user is admin, sends userid via header of api
            {
                return BadRequest("User is not a admin");
            }
            if (!ModelState.IsValid) //check if model is valid
            {
                return BadRequest(ModelState);
            }

            Ingredient temping = db.Ingredient.Where(e => e.name == ingredient.name).FirstOrDefault(); //searches dbo.ingredient to see if the ingredient already exist
            if(temping != null) 
            {
                return BadRequest("That ingredient already exists");  //error message if it exist
            }

            db.Entry(ingredient).State = EntityState.Modified; //tells db what to modify
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("ingredients/newingredient")] // add a new ingredient, object via body
        [ResponseType(typeof(Ingredient))]
        public string PostIngredient(Ingredient ingredient)
        {
            if (!ModelState.IsValid) //if model is valid
            {
                return "invalid model";
            }

            db.Ingredient.Add(ingredient); //add ingredient to db
            db.SaveChanges();

            return "succesfully created " + ingredient.name; //return string
        }

        [HttpDelete]
        [Route("ingredient/delete/{id}/{userid}")] //Delete ingredient
        [ResponseType(typeof(Ingredient))]
        public IHttpActionResult DeleteIngredient(int id, int userid) //id of ingredient and userid to check admin
        {
            User checkifadmin = db.User.Find(userid); 
            if (checkifadmin.role.ToLower() != "admin") //see if user role is admin
            {
                return BadRequest("User is not a admin");
            }
            Ingredient ingredient = db.Ingredient.Find(id); //find the ingredient from dbo.ingredient with id from header of api
            if (ingredient == null)
            {
                return NotFound();
            }

            db.Ingredient.Remove(ingredient); //remove ingredient from db
            db.SaveChanges();

            return Ok(ingredient);
        }

    }
}