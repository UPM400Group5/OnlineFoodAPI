using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    // This model is meant for the get api. password is removed
    public class UserAPIModel
    {
        public int id { get; set; }
        public string role { get; set; }
        public string adress { get; set; }      
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
    }
}