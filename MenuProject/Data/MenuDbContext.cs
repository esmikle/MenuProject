using System;
using MenuProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MenuProject.Data
{
	public class MenuDbContext : DbContext
	{
		public MenuDbContext( DbContextOptions<MenuDbContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			// DishIngredient has 2 keys DishId and IngredientId
			modelBuilder.Entity<DishIngredient>().HasKey(di => new
			{
				di.DishId,
				di.IngredientId
			});

			// One Dish has many relationship to DishIngredients and DishId is a foreign key
			modelBuilder.Entity<DishIngredient>().HasOne(d => d.Dish).WithMany(di => di.DishIngredients).HasForeignKey(d => d.DishId);
			// One Ingredient has many relationship to DishIngredients and IngredientId is a foreign key
            modelBuilder.Entity<DishIngredient>().HasOne(i => i.Ingredient).WithMany(ing => ing.DishIngredients).HasForeignKey(i => i.IngredientId);

			modelBuilder.Entity<Dish>().HasData(
				new Dish { Id = 1, Name = "Hawaiian", Price = 7.50, ImageUrl = "https://www.onelovelylife.com/wp-content/uploads/2024/08/Hawaiian-Pizza20-3.jpg" }
				);
			modelBuilder.Entity<Ingredient>().HasData(
				new Ingredient { Id=1, Name="Mozzarella Cheese"},
				new Ingredient { Id=2, Name="Tomatoe Sauce"},
				new Ingredient { Id=3, Name="Ham"},
				new Ingredient { Id=4, Name="Pineapple"}
				);
			modelBuilder.Entity<DishIngredient>().HasData(
				new DishIngredient { DishId=1, IngredientId=1 },
                new DishIngredient { DishId = 1, IngredientId = 2 },
                new DishIngredient { DishId = 1, IngredientId = 3 },
                new DishIngredient { DishId = 1, IngredientId = 4 }
                );

			base.OnModelCreating(modelBuilder);
        }
    

        public DbSet<Dish> Dishes { get; set; }

		public DbSet<Ingredient> Ingredients { get; set; }

		public DbSet<DishIngredient> DishIngredients { get; set; }
	}
}

