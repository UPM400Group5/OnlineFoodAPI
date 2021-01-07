namespace OnlineFoodAPI
{
    using Newtonsoft.Json;
    using OnlineFoodAPI.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("Restaurant")]
    [DataContract(IsReference = true)]
    [JsonObject(IsReference = false)]
    public partial class Restaurant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
          public Restaurant()
        {
            Dishes = new HashSet<Dishes>();
            User = new HashSet<User>();
        }  
         

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string adress { get; set; }

        [Required]
        [StringLength(50)]
        public string city { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public int delivery_price { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<Dishes> Dishes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<User> User { get; set; } 
        public  IList<FavoritesRestaurants> FavoritesRestaurants { get; set; }

    }
}
