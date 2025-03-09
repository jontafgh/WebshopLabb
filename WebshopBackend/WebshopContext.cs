using Microsoft.AspNetCore.Identity;
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
        public DbSet<Restock> Restocks { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Boardgame>()
                .HasOne(b => b.Product)
                .WithOne();

            modelBuilder.Entity<WebshopUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.Id);
        }
    }
    public class WebshopUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public Cart? Cart { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public List<WebshopUser> Users { get; set; } = [];
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
