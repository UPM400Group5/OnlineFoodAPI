﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    [Table("FavoritesRestaurants")]
    public class FavoritesRestaurants
    {
        [ForeignKey("User")]
        public int User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("Restaurant")]

        public int Restaurant_id { get; set; }

        public virtual Restaurant Restaurant { get; set; }


    }
}