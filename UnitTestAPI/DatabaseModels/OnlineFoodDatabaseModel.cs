using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UnitTestAPI
{
    public partial class OnlineFoodDatabaseModel : DbContext
    {
        public OnlineFoodDatabaseModel()
            : base("name=OnlineFoodUnitTestDatabaseModel")
        {
        }

        public virtual DbSet<Dishes> Dishes { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dishes>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Dishes>()
                .HasMany(e => e.Ingredient)
                .WithMany(e => e.Dishes)
                .Map(m => m.ToTable("DishesIngredient"));

            modelBuilder.Entity<Ingredient>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.adress)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.phonenumber)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .HasMany(e => e.Dishes)
                .WithRequired(e => e.Restaurant)
                .HasForeignKey(e => e.Restaurant_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Restaurant>()
                .HasMany(e => e.User)
                .WithMany(e => e.Restaurant)
                .Map(m => m.ToTable("FavoritesRestaurants"));

            modelBuilder.Entity<User>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.adress)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.city)
                .IsUnicode(false);
        }
    }
}
