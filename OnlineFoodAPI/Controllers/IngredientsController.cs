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
            return db.Ingredient;
        }

        [Route("ingredient/getspecific/{id}")]
        [ResponseType(typeof(Ingredient))]
        public IHttpActionResult GetIngredient(int id)
        {
            Ingredient ingredient = db.Ingredient.Find(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPut]
        [Route("ingredient/update/{id}/{userid}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIngredient(int id, Ingredient ingredient, int userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role.ToLower() != "admin")
            {
                return BadRequest("User is not a admin");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingredient.id)
            {
                return BadRequest();
            }
            Ingredient temping = db.Ingredient.Where(e => e.name == ingredient.name).FirstOrDefault();
            if(temping == null)
            {
                return BadRequest("That ingredient already exists");
            }

            db.Entry(ingredient).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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
        [Route("ingredients/newingredient")]
        [ResponseType(typeof(Ingredient))]
        public string PostIngredient(Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return "invalid model";
            }

            db.Ingredient.Add(ingredient);
            db.SaveChanges();

            return "succesfully created " + ingredient.name;
        }

        [HttpDelete]
        [Route("ingredient/delete/{id}/{userid}")]
        // DELETE: api/Ingredients/5
        [ResponseType(typeof(Ingredient))]
        public IHttpActionResult DeleteIngredient(int id, int userid)
        {
            User checkifadmin = db.User.Find(userid);
            if (checkifadmin.role.ToLower() != "admin")
            {
                return BadRequest("User is not a admin");
            }
            Ingredient ingredient = db.Ingredient.Find(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            db.Ingredient.Remove(ingredient);
            db.SaveChanges();

            return Ok(ingredient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool IngredientExists(int id)
        {
            return db.Ingredient.Count(e => e.id == id) > 0;
        }
    }
}