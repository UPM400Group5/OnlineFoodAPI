using NUnit.Framework;
using OnlineFoodAPI;
using OnlineFoodAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

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
            List<Dishes> listing = controller.GetDishesOld().ToList();
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
                temping = result2.Find(e => e.name == "ost");
                controllerIng.DeleteIngredient(temping.id, userid);
            }
            catch { }
        }

        [Test]
        public void GetDishes_ReturnListOfDishes()
        {
            List<OnlineFoodAPI.Models.Dish> tempdish = controller.GetDishes();
            Assert.IsInstanceOf<List<OnlineFoodAPI.Models.Dish>>(tempdish);
        }

        [Test]
        public void PutDishes_StringNoIngredient()
        {
            IngredientsController ingcontroller = new IngredientsController();
            List<Ingredient> ingredientswithID = ingcontroller.GetIngredient().ToList();
            List<Ingredient> ingredienstodelete = new List<Ingredient>();
            Ingredient temping = item.Ingredient.First();
            ingredienstodelete = ingredientswithID.Where(e => e.name == temping.name).ToList();
            foreach (var ing in ingredienstodelete)
            {
                ingcontroller.DeleteIngredient(ing.id, 2);
            }
            item.Ingredient = null;
            var specialpricenull = controller.PutDishess(item.id, 2, item);
            Assert.AreEqual("There was no ingredient attached", specialpricenull);

        }
        [Test]
        public void PutDishes_StringDishUpdated()
        {

            Dishes PutDish = GetPutDemoDish();
            PutDish.id = item.id;
            var ifchangeddish = controller.PutDishess(PutDish.id, 2, PutDish);
            Assert.AreEqual("updated", ifchangeddish);
        }

        [Test]
        public void PutDishes_StringUserWrongDishIDWrong()
        {
            var checkifadmin = controller.PutDishess(item.id, 1, item);
            Assert.IsInstanceOf<string>(checkifadmin);
            var checkifdishidistrue = controller.PutDishess(2, 2, item);
            Assert.AreEqual("id doesnt exist", checkifdishidistrue);
        }


        [Test]
        public void GetDishes_ReturnOkHttp()
        {
            var dishesnull = controller.GetDishes(2);
            Assert.IsInstanceOf<NotFoundResult>(dishesnull);
            var returnok = controller.GetDishes(item.id);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Dishes>>(returnok);

        }

        [Test]
        public void AddIngredientToDish_CheckadminandIdWrong()
        {
            Ingredient[] ingredientArray = GetIngredients().ToArray();
            var usernotadmin = controller.AddIngredientToDish(item.id, 1, ingredientArray);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(usernotadmin, "User is not a admin");

            var Iddoesntextst = controller.AddIngredientToDish(1, 2, ingredientArray);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(Iddoesntextst, "id doesnt exist");

        }

        [Test]
        public void AddIngredientToDish_ReturnOkHttp()
        {
            //TODO: krånglar
            Ingredient[] ingredientArray = GetIngredients().ToArray();
            var Sucessadd = controller.AddIngredientToDish(item.id, 2, ingredientArray);
            Assert.IsInstanceOf<Dishes[]>(Sucessadd);
        }

        [Test]
        public void GetDishes_NotNull()
        {
            // New 
            Assert.IsNotNull(controller.GetDishes());
        }

        [Test]
        public void GetTotalPriceOfBasket_returnPriceAsString()
        {
            List<Dishes> tempdishlist = new List<Dishes>();
            tempdishlist.Add(item); tempdishlist.Add(item);
            var return200string = controller.GetTotalPriceOfBasket(tempdishlist);
            Assert.AreEqual("200", return200string);
            item.specialprice = 50;
            tempdishlist.Clear();
            tempdishlist.Add(item); tempdishlist.Add(item);
            var return100string = controller.GetTotalPriceOfBasket(tempdishlist);
            Assert.AreEqual("100", return100string);
            tempdishlist.Clear();
            item.price = 0; item.specialprice = null;
            tempdishlist.Add(item);
            var returnStringError = controller.GetTotalPriceOfBasket(tempdishlist);
            Assert.AreEqual("something isn't quite right, ERROR: price isnt set", returnStringError);

        }

        [Test]
        public void PostDishes_BadModelandUserNotAdmin()
        {
            var usernotadmin = controller.PostDishes(item, 1);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(usernotadmin, "User is not a admin");
            controller.ModelState.AddModelError("test", "test");
            var badModel = controller.PostDishes(item, 2);
            Assert.IsInstanceOf<InvalidModelStateResult>(badModel);
        }

        [Test]
        public void DeleteDishes_NoDishIDandUserNotAdmin()
        {
            var usernotadmin = controller.DeleteDishes(item.id, 1);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(usernotadmin, "User is not a admin");
            var dishiddoesntexist = controller.DeleteDishes(1, 2);
            Assert.IsInstanceOf<NotFoundResult>(dishiddoesntexist);
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
        public Ingredient[] GetIngredients()
        {
            Ingredient[] ingredient = new Ingredient[3];
            ingredient[0] = new Ingredient() { name = "tomatsås" };
            ingredient[1] = new Ingredient() { name = "PepperoniUppdaterat" };
            ingredient[2] = new Ingredient() { name = "ostUppdaterat" };
            return ingredient;
        }
    }
}
