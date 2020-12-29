using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;
using OnlineFoodAPI;
using System.Web.Http;
using System.Web.Http.Results;

namespace UnitTestAPI
{
    [TestClass]
    public class IngredientTest
    {

        [TestMethod]
        public void AddIngredient()
        {

            var controller = new IngredientsController();

            var item = GetDemoIngredient();

            var result =
                controller.PostIngredient(item);


            Assert.AreEqual(result, "succesfully created DemoIngredient");

        }

        [TestMethod]
        public void GetIngredients_ReturnsListOfIngredients()
        {
            var controller = new IngredientsController();
            var item = controller.GetIngredient();
            Assert.IsNotNull(item);
        }
        
        [TestMethod]
        public void GetSpecificIngredient_returnsOneIngredient()
        {
            var controller = new IngredientsController();
            var id = 1;
            IHttpActionResult actionResult = controller.GetIngredient(id);
            var contentresult = actionResult as OkNegotiatedContentResult<Ingredient>;
            //if id are the same
            Assert.AreEqual(id, contentresult.Content.id);
        }
        [TestMethod]
        public void GetSpecificIngredient_Fails()
        {
            var controller = new IngredientsController();
            var id = 4;
            IHttpActionResult actionresult = controller.GetIngredient(id);

            Assert.IsInstanceOfType(actionresult, typeof(NotFoundResult));

        }
        [TestMethod]
        public void PutIngredient_BadRequestFoodExist()
        {
            var controller = new IngredientsController();
            var userid = 2;
            Ingredient demoing = GetDemoPutExistingIngredient();
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOfType(actionresult, typeof(BadRequestErrorMessageResult));
        }
        /*[TestMethod]
        public void PutIngredient_BadRequest()
        {
            var controller = new IngredientsController();
            var userid = 2;
            Ingredient demoing = GetDemoBadPutIngredient();
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOfType(actionresult, typeof(BadRequestErrorMessageResult));
        } */
        [TestMethod]
        public void PutIngredient_BadRequestUserIsNotAdmin()
        {
            var controller = new IngredientsController();
            var userid = 1;
            Ingredient demoing = GetDemoBadPutIngredient();
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            Assert.IsInstanceOfType(actionresult, typeof(BadRequestErrorMessageResult));
        }
        [TestMethod]
        public void PutIngredient_StatusCodeSuccesfullPut() //has to .name each time.
        {
            var controller = new IngredientsController();
            var userid = 2;
            Ingredient demoing = GetDemoPutIngredient();
            demoing.name = "12347"; //Update each time
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);
            updateBackDb();
            Assert.IsInstanceOfType(actionresult, typeof(StatusCodeResult));
            
        }
        [TestMethod]
        public void DelteIngredient_statusOKSucess() //has to change id each time
        {
            var controller = new IngredientsController();
            var userid = 2;
            var id = 58; //has to be changed each time?
            IHttpActionResult actionresult = controller.DeleteIngredient(id, userid);
            Assert.IsInstanceOfType(actionresult, typeof(OkNegotiatedContentResult<Ingredient>));
        }
        [TestMethod]
        public void DeleteIngredient_BadRequestUserNotAdmin()
        {
            var controller = new IngredientsController();
            var userid = 1;
            var id = 50;
            IHttpActionResult actionresult = controller.DeleteIngredient(id, userid);
            Assert.IsInstanceOfType(actionresult, typeof(BadRequestErrorMessageResult));
        }
        [TestMethod]
        public void DeleteIngredient_BadRequestDishDoestNotExist()
        {
            var controller = new IngredientsController();
            var userid = 2;
            var id = 4;
            IHttpActionResult actionresult = controller.DeleteIngredient(id, userid);
            Assert.IsInstanceOfType(actionresult, typeof(NotFoundResult));
        }



        Ingredient GetDemoIngredient()
        {
            return new Ingredient() { id = 52, name = "DemoIngredient" };
        }
        Ingredient GetDemoPutIngredient()
        {
            return new Ingredient() { id = 2, name = "DemoCheese3"  };
        }
        Ingredient GetDemoBadPutIngredient()
        {
            return new Ingredient() { id = 3, name = "pepperoni" };

        }
        Ingredient GetDemoPutExistingIngredient()
        {
            return new Ingredient() { id = 3, name = "Pesto" };
        }
        void updateBackDb()
        {
            var controller = new IngredientsController();
            Ingredient demoing = GetDemoBadPutIngredient();
            demoing.name = "democheese";
            var userid = 2;
            IHttpActionResult actionresult = controller.PutIngredient(demoing, userid);

        }

    }
    #region old
    //[TestMethod]
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

    //[TestMethod]
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
