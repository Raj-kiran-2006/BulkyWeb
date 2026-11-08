using Bulky.Models;
using BulkyWeb.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
         {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products{ get; set; }

        protected  override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 2,
                    ImageUrl = "/Images/Books/fortune-of-time.svg"
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = "/Images/Books/dark-skies.svg"
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 2,
                    ImageUrl = "/Images/Books/vanish-in-the-sunset.svg"
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 1,
                    ImageUrl = "/Images/Books/cotton-candy.svg"
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = "/Images/Books/rock-in-the-ocean.svg"
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNulla malesuada sollicitudin risus, vitae suscipit eros aliquet quis. Vivamus ultrices velit eget diam viverra viverra. Praesent vel justo vel erat ullamcorper lacinia. Aenean placerat risus in nisi pellentesque commodo eros imperdiet.",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId=1,
                    ImageUrl = "/Images/Books/leaves-and-wonders.svg"
                },
                new Product
                {
                    Id = 7,
                    Title = "The Glass Meridian",
                    Author = "Mira Sen",
                    Description = "When a lighthouse begins casting a second shadow, cartographer Elian discovers a hidden coastline drawn across the night sky. Following its shifting line leads to a quiet town that appears on no map and remembers a future no one has lived.",
                    ISBN = "BULKY000007",
                    ListPrice = 48,
                    Price = 42,
                    Price50 = 38,
                    Price100 = 34,
                    CategoryId = 2,
                    ImageUrl = "/Images/Books/the-glass-meridian.svg"
                },
                new Product
                {
                    Id = 8,
                    Title = "A Map of Quiet Stars",
                    Author = "Arun Vale",
                    Description = "After inheriting an old observatory, Nila finds a hand-drawn map that marks forgotten places between the stars. Each journey reveals a small act of courage on Earth, and a choice that may bring a scattered family back together.",
                    ISBN = "BULKY000008",
                    ListPrice = 52,
                    Price = 46,
                    Price50 = 41,
                    Price100 = 37,
                    CategoryId = 3,
                    ImageUrl = "/Images/Books/a-map-of-quiet-stars.svg"
                }
            );
        }
    }
}
