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

            var app = builder.Build();

            app.MapGet("/products", async (WebshopContext db) =>
            
                await db.Boardgames
                   .Include(b => b.Publisher)
                   .Include(b => b.Image)
                   .Include(b => b.Price!)
                       .ThenInclude(p => p.Discount)
                   .Include(b => b.BoardgameDetails)
                   .Include(b => b.Stock!)
                       .ThenInclude(s => s.NextRestock)
                   .ToListAsync()
                   is List<Boardgame> boardgame
                        ? Results.Ok(boardgame)
                        : Results.NotFound()
            );

            app.MapGet("/products/{id:int}", async (int id, WebshopContext db) =>
                await db.Boardgames.Include(b => b.Publisher)
                    .Include(b => b.Image)
                    .Include(b => b.Price!)
                        .ThenInclude(p => p.Discount)
                    .Include(b => b.BoardgameDetails)
                    .Include(b => b.Stock!)
                        .ThenInclude(s => s.NextRestock)
                    .FirstOrDefaultAsync(b => b.Id == id)
                    is Boardgame boardgame
                        ? Results.Ok(boardgame)
                        : Results.NotFound()
            );

            app.MapGet("/products/article/{artNr}", async (string artNr, WebshopContext db) =>
                await db.Boardgames.Include(b => b.Publisher)
                        .Include(b => b.Image)
                        .Include(b => b.Price!)
                        .ThenInclude(p => p.Discount)
                        .Include(b => b.BoardgameDetails)
                        .Include(b => b.Stock!)
                        .ThenInclude(s => s.NextRestock)
                        .FirstOrDefaultAsync(b => b.ArtNr == artNr)
                    is Boardgame boardgame
                    ? Results.Ok(boardgame)
                    : Results.NotFound()
            );

            app.MapPost("/products", async (BoardgameDto boardgameDto, WebshopContext db) =>
            {
                var boardgame = boardgameDto.ToBoardgame();
                db.Boardgames.Add(boardgame);
                await db.SaveChangesAsync();

                return Results.Created($"/products/{boardgame.Id}", boardgame);
            });

            app.Run();
        }
    }
}
