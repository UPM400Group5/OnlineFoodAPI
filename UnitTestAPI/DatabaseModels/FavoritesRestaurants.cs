using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnitTestAPI.Models
{
    [Table("FavoritesRestaurants")]
    public class FavoritesRestaurants
    {
        [ForeignKey("User")]
        public int User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("Restaurant")]

        public int Restaurant_id { get; set; }

        public  Restaurant Restaurant { get; set; }


    }
}