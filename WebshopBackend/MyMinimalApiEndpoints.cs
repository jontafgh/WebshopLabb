using Microsoft.EntityFrameworkCore;
using WebshopBackend.Models;
using WebshopShared;
namespace WebshopBackend
{
    public static class MyMinimalApiEndpoints
    {
        public static void MapMinimalApiEndpoints(this WebApplication app)
        {
            app.MapGet("/products", async (WebshopContext db) =>
                await db.Products.Include(p => p.Image)
                    .Include(p => p.Price)
                        .ThenInclude(p => p.Discount)
                    .Include(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                    .ToListAsync()
                    is { } product
                    ? Results.Ok(product)
                    : Results.NotFound()
                );

            app.MapGet("/products/{id:int}", async (int id, WebshopContext db) =>
                await db.Products.Include(p => p.Image)
                    .Include(p => p.Price)
                        .ThenInclude(p => p.Discount)
                    .Include(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                    .FirstOrDefaultAsync(p => p.Id == id)
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

            app.MapPost("/cart", async (CreateCartDto createCartDto, WebshopContext db) =>
            {
                var cart = createCartDto.ToCart();
                db.Carts.Add(cart);
                await db.SaveChangesAsync();
                return Results.Created($"/cart/{cart.Id}", cart);
            });

            app.MapGet("/cart/{id:int}/cartitems/", async (int id, WebshopContext db) =>
            {
                var cart = await db.CartItems.Where(ci => ci.CartId == id)
                    .Include(p => p.Product)
                    .ThenInclude(p => p.Price)
                    .ThenInclude(d => d.Discount)
                    .Select(c => c.ToCartItemDto())
                    .ToListAsync();
                return Results.Ok(cart);
            });

            app.MapPut("/cart/{cartid:int}", async (int cartid, List<CartItemDto> cartItems, WebshopContext db) =>
            {
                var cart = await db.Carts.Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.Id == cartid);
                if (cart is null)
                {
                    return Results.NotFound();
                }
                cart.CartItems = cartItems.Select(ci => ci.ToCartItem()).ToList();
                await db.SaveChangesAsync();
                return Results.Ok(cart);
            });
        }
    }
}
