using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    public class OrderModel
    {
        public List<Dishes> dishes { get; set; }
        public int userId { get; set; }
    }
}