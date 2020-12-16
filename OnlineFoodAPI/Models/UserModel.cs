using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFoodAPI.Models
{
    // This model is meant for the get api. password is removed
    public class UserModel
    {
        public int id { get; set; }
        public string role { get; set; }
        public string adress { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public UserModel(User user)
        {
            this.id = user.id;
            this.role = user.role;
            this.adress = user.adress;
            this.city = user.city;
            this.email = user.email;
            this.phone = user.phone;
            this.username = user.username;
        }
    }
}
