﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebshopBackend.Models;

namespace WebshopBackend
{
    public class WebshopContext : IdentityDbContext<WebshopUser>
    {
        public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) {}
       
        public DbSet<Boardgame> Boardgames { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Boardgame>()
                .HasOne(b => b.Product)
                .WithOne();
        }
    }
    public class WebshopUser : IdentityUser
    {
        
    }
}
