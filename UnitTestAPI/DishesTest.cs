using NUnit.Framework;
using OnlineFoodAPI;
using OnlineFoodAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestAPI
{
    public class DishesTest
    {
        DishesController controller;
        Dishes item;
        int userid;
        [SetUp]
        public void Setup()
        {
            controller = new DishesController();
            userid = 2;
            item = GetDemoDish();
            var result =
                controller.PostDishes(item, userid);
            List<Dishes> listing = controller.GetDishes().ToList();
            var dishes = listing[listing.Count() - 1];
            item.id = dishes.id;
        }
        [TearDown]
        public void Teardown()
        {
            try
            {
                userid = 2;
                controller.ModelState.Clear();

                var result =
                    controller.DeleteDishes(item.id, userid);
                IngredientsController controllerIng = new IngredientsController();
                var result2 =
                    controllerIng.GetIngredient().ToList();
                Ingredient temping = result2.Find(e=>e.name == "testpepperoni");
                controllerIng.DeleteIngredient(temping.id, userid);
            }
            catch { }
        }

        [Test]
        public void GetDishes_ReturnListOfDishes()
        {
            List<Dishes> tempdish = controller.GetDishes();
            Assert.IsInstanceOf<List<Dishes>>(tempdish);
        }
        [Test]
        public void PutDishes_StringwithSuccess()
        {
            var checkifadmin = controller.PutDishess(item.id, 1, item);
            Assert.IsInstanceOf<string>(checkifadmin);
            var checkifdishidistrue = controller.PutDishess(2, 2, item);
            Assert.AreEqual("id doesnt exist", checkifdishidistrue);
            var completeDish = controller.PutDishess(item.id, 2, item);
            Assert.IsInstanceOf<string>(completeDish);
            item.Ingredient = null;
            var specialpricenull = controller.PutDishess(item.id, 2, item);
            Assert.AreEqual("There was no ingredient attached", specialpricenull);
            Dishes PutDish = GetPutDemoDish();
            PutDish.id = item.id;
            var ifchangeddish = controller.PutDishess(PutDish.id, 2, PutDish);
            Assert.AreEqual("updated", ifchangeddish);
        }






        public Dishes GetDemoDish()
        {
            Dishes dish = new Dishes() { name = "testing", Restaurant_id = 2, price = 100, specialprice = 0 };
            Ingredient[] ingredient = new Ingredient[2];
            ingredient[0] = new Ingredient() { name="tomatsås"};
            ingredient[1] = new Ingredient() { name = "testpepperoni" };
            dish.Ingredient = ingredient;
            return dish;
        }
        public Dishes GetPutDemoDish()
        {
            Dishes dish = new Dishes() { name = "testing", Restaurant_id = 2, price = 100, specialprice = 50 };
            Ingredient[] ingredient = new Ingredient[3];
            ingredient[0] = new Ingredient() { name = "tomatsås" };
            ingredient[1] = new Ingredient() { name = "testpepperoni" };
            ingredient[2] = new Ingredient() { name = "ost" };
            dish.Ingredient = ingredient;
            return dish;
        }
    }
}
