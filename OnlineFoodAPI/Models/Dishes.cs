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

    [DataContract(IsReference = true)]
    [JsonObject(IsReference = false)]
    public partial class Dishes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dishes()
        {
            Ingredient = new HashSet<Ingredient>();
            User = new HashSet<User>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [ForeignKey("Restaurant")]
        public int Restaurant_id { get; set; }

        public int price { get; set; }
        public int? specialprice { get; set; }

        public  Restaurant Restaurant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Ingredient> Ingredient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<User> User { get; set; }
        public  IList<DishesIngredient> DishesIngredient { get; set; }

    }
}
