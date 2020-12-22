using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using OnlineFoodAPI.Models;

namespace OnlineFoodAPI.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        /// <summary>
        /// Compares username and password. If both matches someone in the database an object will be returned.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">The password</param>
        public UserModel LoginUser(LoginModel loginDetails) 
        {
            try
            {
                using (DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel())
                {
                    // If username and password fits a person in the database
                    var user = db.User.Where(x => x.username == loginDetails.username && x.password == loginDetails.password).FirstOrDefault();

                    if (user != null)
                    {
                        // Returns a custom model
                        UserModel model = new UserModel(user);
                        return model;
                    }
                }
            }
            catch 
            {
               
            }
            // Return null if invalid or failed
            return null;
        }
    }
}
