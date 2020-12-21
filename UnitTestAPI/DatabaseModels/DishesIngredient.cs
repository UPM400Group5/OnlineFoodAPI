﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnitTestAPI.Models
{
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