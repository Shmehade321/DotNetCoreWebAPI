using System;
using DotNetCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreWebAPI.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Shirt> Shirts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data Seeding
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { Id = 1, Brand = "Nike", Color = "Black", Gender = "men", Price = 29.99, Size = 7 },
                new Shirt { Id = 2, Brand = "Addidas", Color = "Yellow", Gender = "women", Price = 59.99, Size = 10 },
                new Shirt { Id = 3, Brand = "Atletics", Color = "Brown", Gender = "men", Price = 19.99, Size = 9 },
                new Shirt { Id = 4, Brand = "Puma", Color = "White", Gender = "women", Price = 39.99, Size = 12 }
            );
        }
    }
}

