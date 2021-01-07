using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OnlineFoodAPI.Models
{
    [DataContract(IsReference = false)]
    [Serializable()]
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}