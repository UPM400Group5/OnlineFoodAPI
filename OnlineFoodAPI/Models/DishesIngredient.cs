using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OnlineFoodAPI.Models
{

    [DataContract(IsReference = true)]
    [Serializable()]
    [Table("DishesIngredient")]
    public class DishesIngredient
    {
        [ForeignKey("Dishes")]
        public int Dishes_id { get; set; }
        public Dishes Dishes { get; set; }

        [ForeignKey("Ingredient")]
        public int Ingredient_id { get; set; }
        public Ingredient Ingredient { get; set; }
    }

}