using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    public class ReceiptModel
    {
        public string userAdress { get; set; }
        public int userId { get; set; }
        public string userUsername { get; set; }
        public string userEmail { get; set; }
        public string userCity { get; set; }
        public string userPhone { get; set; }
        public int sumTotal { get; set; }
        public int delivery_price { get; set; }
        public List<Dishes> dishes { get; set; }
        public string restaurantName { get; set; }
    }
}