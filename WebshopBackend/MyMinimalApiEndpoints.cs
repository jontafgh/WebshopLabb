using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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

            app.MapPost("/cart", async (ClaimsPrincipal claims, WebshopContext db, [FromBody] object? empty) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var existingCart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

                if (existingCart is not null)
                {
                    return Results.Conflict();
                }

                var createCartDto = new CreateCartDto { UserId = userId };
                var cart = createCartDto.ToCart();

                db.Carts.Add(cart);
                await db.SaveChangesAsync();

                return Results.Created($"/cart/{cart.Id}", cart);

            }).RequireAuthorization();

            app.MapGet("/cart/cartitems/", async (ClaimsPrincipal claims, WebshopContext db) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var cartId = await db.Carts.Where(c => c.UserId == userId)
                    .AsNoTracking()
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();

                if (cartId == 0)
                {
                    return Results.NotFound();
                }

                var cart = await db.CartItems.Where(ci => ci.CartId == cartId)
                    .AsNoTracking()
                    .Include(p => p.Product)
                    .ThenInclude(p => p.Price)
                    .ThenInclude(d => d.Discount)
                    .Select(c => c.ToCartItemDto())
                    .ToListAsync();

                return Results.Ok(cart);

            }).RequireAuthorization();

            app.MapPut("/cart", async (ClaimsPrincipal claims, List<CartItemDto> cartItems, WebshopContext db) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var cart = await db.Carts.Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart is null)
                {
                    return Results.NotFound();
                }

                cart.CartItems = cartItems.Select(ci => ci.ToCartItem()).ToList();
                await db.SaveChangesAsync();
                return Results.Ok(cart);

            }).RequireAuthorization();

            app.MapDelete("/cart", async (ClaimsPrincipal Claim, WebshopContext db) =>
            {
                var userId = Claim.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                var cart = await db.Carts.Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart is null)
                {
                    return Results.NotFound();
                }
                db.Carts.Remove(cart);
                await db.SaveChangesAsync();
                return Results.Ok();

            }).RequireAuthorization();

            app.MapPost("/order", async (ClaimsPrincipal claims, PlaceOrderDto placeOrderDto, WebshopContext db) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var order = placeOrderDto.ToOrder(userId);
                db.Orders.Add(order);
                await db.SaveChangesAsync();

                return Results.Created($"/order/{order.Id}", order);

            }).RequireAuthorization();

            app.MapGet("/order/{orderId:int}", async (int orderId, ClaimsPrincipal claims, WebshopContext db) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var order = await db.Orders.Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                    .ThenInclude(p => p.Price)
                    .ThenInclude(d => d.Discount)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

                return order == null ? Results.NotFound() : Results.Ok(order.ToOrderDto());

            }).RequireAuthorization();

            app.MapGet("/orders", async (ClaimsPrincipal claims, WebshopContext db) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                var orders = await db.Orders
                    .Where(o => o.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();

                return Results.Ok(orders);

            }).RequireAuthorization();
        }
    }
}
