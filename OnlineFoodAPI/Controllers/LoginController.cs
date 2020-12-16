﻿using System;
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
        public UserModel LoginUser(LoginModel loginDetails) 
        {
            try
            {
                using (DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel())
                {
                    var user = db.User.Where(x => x.username == loginDetails.username && x.password == loginDetails.password).FirstOrDefault();

                    if (user != null)
                    {
                        // Returns a custom model
                        UserModel model = new UserModel(user);
                        return model;
                    }

                    // Return null if invalid
                    return null;
                }
            }
            catch 
            {   
                // If error, return null
                return null;
            }
            
        }
    }
}