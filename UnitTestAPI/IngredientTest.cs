using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using OnlineFoodAPI.Controllers;
using OnlineFoodAPI.Models;
using OnlineFoodAPI;

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
        Ingredient GetDemoIngredient()
        {
            return new Ingredient() { id = 2, name = "DemoIngredient" };
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
