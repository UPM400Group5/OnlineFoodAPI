using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    public class Dish
    {
        public int id { get; set; }
        public string name { get; set; }
        public int Restaurant_id { get; set; }
        public int price { get; set; }
        public int? specialprice { get; set; }
        public List<IngredientModel> ingredients { get; set; }

        public Dish(Dishes dish) 
        {
            this.id = dish.id;
            this.name = dish.name;
            this.Restaurant_id = dish.Restaurant_id;
            this.price = dish.price;
            this.specialprice = dish.specialprice;
        }
    }
    public class IngredientModel 
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}