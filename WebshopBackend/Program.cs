using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebshopBackend.Models;
using WebshopShared;
using static System.Net.WebRequestMethods;
namespace WebshopBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddDbContext<WebshopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebshopDb"))
                );

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            var app = builder.Build();

            app.MapGet("/products", async (WebshopContext db) => 
                await db.Products.Include(p => p.Image)
                    .Include(p => p.Price)
                        .ThenInclude(p => p.Discount)
                    .Include(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                        
                    .Select(p => p.ToProductDto()).ToListAsync()
                    is { } product
                    ? Results.Ok(product)
                    : Results.NotFound()
                );

            app.MapGet("/boardgames", async (WebshopContext db) =>

                await db.Boardgames.Include(b => b.Publisher)
                        .Include(b => b.Product!)
                        .ThenInclude(b => b.Image)
                        .Include(b => b.Product!)
                        .ThenInclude(p => p.Price)
                        .ThenInclude(p => p.Discount)
                        .Include(b => b.Product!)
                        .ThenInclude(p => p.Stock!)
                        .ThenInclude(s => s.NextRestock)
                   .ToListAsync()
                   is { } boardgame
                        ? Results.Ok(boardgame)
                        : Results.NotFound()
            );

            app.MapGet("/boardgames/{id:int}", async (int id, WebshopContext db) =>
                await db.Boardgames.Include(b => b.Publisher)
                        .Include(b => b.Product!)
                            .ThenInclude(b => b.Image)
                        .Include(b => b.Product!)
                            .ThenInclude(p => p.Price)
                                .ThenInclude(p => p.Discount)
                        .Include(b => b.Product!)
                            .ThenInclude(p => p.Stock!)
                                .ThenInclude(s => s.NextRestock)
                    .FirstOrDefaultAsync(b => b.Id == id)
                    is { } boardgame
                        ? Results.Ok(boardgame)
                        : Results.NotFound()
            );

            app.MapGet("/boardgames/article/{artNr}", async (string artNr, WebshopContext db) =>
                await db.Boardgames.Include(b => b.Publisher)
                        .Include(b => b.Product!)
                            .ThenInclude(b => b.Image)
                        .Include(b => b.Product!)
                            .ThenInclude(p => p.Price!)
                                .ThenInclude(p => p.Discount)
                        .Include(b => b.Product!)
                            .ThenInclude(p => p.Stock!)
                                .ThenInclude(s => s.NextRestock)
                        .FirstOrDefaultAsync(b => b.Product!.ArtNr == artNr)
                    is { } boardgame
                    ? Results.Ok(boardgame)
                    : Results.NotFound()
            );

            app.MapPost("/boardgames", async (BoardgameDto boardgameDto, WebshopContext db) =>
            {
                var boardgame = boardgameDto.ToBoardgame();
                db.Boardgames.Add(boardgame);
                await db.SaveChangesAsync();

                return Results.Created($"/boardgames/{boardgame.Id}", boardgame);
            });

            app.Run();
        }
    }
}
