using System;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;
using OnlineFoodAPI;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTestAPI
{
    
    public class IngredientTest
    {
        IngredientsController controller;
        Ingredient item;
        int userid = 2;

        [SetUp]
        public void Setup()
        {
            controller= new IngredientsController();

            item = GetDemoIngredient();
            var result =
                controller.PostIngredient(item);
            List<Ingredient> listing = controller.GetIngredient().ToList();
            var ingredient = listing[listing.Count() - 1];
            item.id = ingredient.id;
        }
        [TearDown]
        public void Teardown()
        {
            try
            {
                userid = 2;
                controller.ModelState.Clear();

                var result =
                    controller.DeleteIngredient(item.id, userid);
            }
            catch { }

        }

        [Test]
        public void PutIngredient_StatusCodeSuccesfullPut() //has to .name each time.
        {
            item.name = "NewNameInPut"; //Update each time
            IHttpActionResult actionresult = controller.PutIngredient(item, userid);
            Assert.IsInstanceOf<StatusCodeResult>(actionresult);

        }
        [Test]
        public void PostIngredient_BadModel() //has to .name each time.
        {
            Ingredient demoing = GetDemoBadPutIngredient();
            controller.ModelState.AddModelError("test", "test");
            string actionresult = controller.PostIngredient(demoing);
            Assert.IsInstanceOf<string>(actionresult);

        }


        [Test]
        public void GetIngredients_ReturnsListOfIngredients()
        {
            var item = controller.GetIngredient();
            Assert.IsNotNull(item);
        }
        
        [Test]
        public void GetSpecificIngredient_returnsOneIngredient()
        {

            IHttpActionResult actionResult = controller.GetIngredient(item.id);
            var contentresult = actionResult as OkNegotiatedContentResult<Ingredient>;
            //if id are the same
            Assert.AreEqual(item.id, contentresult.Content.id);
        }
        [Test]
        public void GetSpecificIngredient_Fails()
        {
            var id = Int32.MaxValue;
            IHttpActionResult actionresult = controller.GetIngredient(id);
            Assert.IsInstanceOf<NotFoundResult>(actionresult);

        }
        [Test]
        public void PutIngredient_BadRequestFoodExist()
        {
            Ingredient demoing = GetDemoPutExistingIngredient();
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionresult);
        }

        [Test]
        public void PutIngredient_BadRequestUserIsNotAdmin()
        {
           userid = 1;
            Ingredient demoing = GetDemoBadPutIngredient();
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionresult);
        } 

        [Test]
        public void PutIngredient_BadRequestModelNotValid()
        {
            Ingredient demoing = GetDemoBadPutIngredient();
            controller.ModelState.AddModelError("test", "test");
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOf<InvalidModelStateResult>(actionresult);
        }

        [Test]
        public void DeleteIngredient_statusOKSucess() 
        {
            IHttpActionResult actionresult = controller.DeleteIngredient(item.id, userid);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Ingredient>>(actionresult);
            List<Ingredient> listing = controller.GetIngredient().ToList();
            bool existiteminlist = listing.Exists(x => x.id == item.id);
            Assert.IsFalse(existiteminlist);

        }
         [Test]
        public void DeleteIngredient_BadRequestUserNotAdmin()
        {
            userid = 1;
            var id = 50;
            IHttpActionResult actionresult = controller.DeleteIngredient(id, userid);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actionresult);
        }
        [Test]
        public void DeleteIngredient_BadRequestDishDoestNotExist()
        {
            var id = Int32.MaxValue;
            IHttpActionResult actionresult = controller.DeleteIngredient(id, userid);
            Assert.IsInstanceOf<NotFoundResult>(actionresult);

        } 
        Ingredient GetDemoIngredient()
        {
            return new Ingredient() { id = 52, name = "DemoIngredient" };
        }
        Ingredient GetDemoPutIngredient()
        {
            return new Ingredient() { id = 2, name = "DemoCheese3"};
        }
        Ingredient GetDemoBadPutIngredient()
        {
            return new Ingredient() { id = 3, name = "pepperoni" };

        }
        Ingredient GetDemoPutExistingIngredient()
        {
            return new Ingredient() { id = 3, name = "Pesto" };
        }


    }
    #region old
    //[Test]
    //public void UpdateIngredient_WithNameOfObject()
    //{

    //    using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
    //    {
    //        Ingredient ingredient = new Ingredient
    //        {
    //            name = "UnitIngredientTest",
    //        };
    //        ingredient = db.Ingredient.Where(e => e.name == ingredient.name).FirstOrDefault();
    //        ingredient.name = "UnitIngredientTestUPDATED";

    //        db.Entry(ingredient).State = EntityState.Modified;
    //        db.SaveChanges();
    //        Assert.IsNotNull(ingredient);
    //    }

    //}

    //[Test]
    //public void DeleteIngredient_ReturnOK()
    //{
    //    var mock

    //    using (OnlineFoodDatabaseModel db = new OnlineFoodDatabaseModel())
    //    {
    //        Ingredient ingredient = new Ingredient
    //        {
    //            name = "UnitIngredientTest",
    //        };
    //        ingredient = db.Ingredient.Where(e => e.name == ingredient.name).FirstOrDefault();
    //        ingredient.name = "UnitIngredientTestUPDATED";

    //        db.Entry(ingredient).State = EntityState.Modified;
    //        db.SaveChanges();
    //        Assert.IsNotNull(ingredient);
    //    }

    //}
    #endregion

   
}
