﻿using Microsoft.EntityFrameworkCore;
using WebshopBackend.Models;

namespace WebshopBackend
{
    public class WebshopContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Boardgame> Boardgames { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Boardgame> Boardgame{ get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boardgame>()
                .HasOne(b => b.Product)
                .WithOne();
        }
    }
}
