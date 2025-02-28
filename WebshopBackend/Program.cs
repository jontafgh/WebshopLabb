using WebshopShared;
using static System.Net.WebRequestMethods;
namespace WebshopBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            
            app.MapGet("/products", () => TypedResults.Ok(ProductExtensions.GetProducts()));
            app.MapGet("/products/{id}", (int id) =>
            {
                var product = ProductExtensions.GetProduct(id);
                return TypedResults.Ok(product);
            });
            app.Run();
        }
    }

    public static class ProductExtensions
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product() { ID = 1, ArtNr = "1234", Name = "Catan", Publisher = "Kosmos", MinPlayers = 3, MaxPlayers = 4, MinAge = 10, PlayTime = 60, Description = "A great game", Price = 30.00m, DiscountedPrice = null, Image = "https://alphaspel.se/media/products/thumbs/d7819003-df98-4149-94d4-7d3ca4e88093.250x250_q50_fill.png", ImageText = "Catan" , Stock = 10 },
                new Product() { ID = 2, ArtNr = "5678", Name = "Ticket to Ride", Publisher = "Days of Wonder", MinPlayers = 2, MaxPlayers = 5, MinAge = 8, PlayTime = 60, Description = "Another great game", Price = 40.00m, DiscountedPrice = 30.00m, Image = "https://alphaspel.se/media/products/thumbs/e42ccac9-f1f2-4640-84d2-3fcf341fea5e.250x250_q50_fill.jpg", ImageText = "Ticket to ride", Stock = 5 }
            };
        }
        public static Product? GetProduct(int id)
        {
            return GetProducts().FirstOrDefault(p => p.ID == id);
        }
    }
}
