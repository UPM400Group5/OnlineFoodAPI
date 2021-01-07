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

    [Table("Ingredient")]
    [DataContract(IsReference = true)]
    [JsonObject(IsReference = false)]

    public partial class Ingredient
    {

        public Ingredient()
        {
            Dishes = new HashSet<Dishes>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Dishes> Dishes { get; set; }
        public IList<DishesIngredient> DishesIngredient { get; set; }
    }
}
