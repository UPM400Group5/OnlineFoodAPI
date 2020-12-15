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
    public class DishesController : ApiController
    {
        private DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel();

        // GET: api/Dishes
        public IQueryable<Dishes> GetDishes()
        {
            return db.Dishes;
        }

        // GET: api/Dishes/5
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

        // PUT: api/Dishes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDishes(int id, Dishes dishes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dishes.id)
            {
                return BadRequest();
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

        // POST: api/Dishes
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult PostDishes(Dishes dishes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dishes.Add(dishes);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dishes.id }, dishes);
        }

        // DELETE: api/Dishes/5
        [ResponseType(typeof(Dishes))]
        public IHttpActionResult DeleteDishes(int id)
        {
            Dishes dishes = db.Dishes.Find(id);
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