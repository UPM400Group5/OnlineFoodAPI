using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    [Table("DishesIngredient")]
    public class DishesIngredient
    {
        [ForeignKey("Dishes")]
        public int Dishes_id { get; set; }
        [ForeignKey("Ingredient")]
        public Dishes Dishes { get; set; }

        [ForeignKey("Ingredient")]
        public int Ingredient_id { get; set; }
        [ForeignKey("Ingredient")]
        public Ingredient Ingredient { get; set; }
    }

}