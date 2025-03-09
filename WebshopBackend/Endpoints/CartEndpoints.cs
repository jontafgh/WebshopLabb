using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Endpoints;

public class CartEndpoints(ICartService cartService, IUserService userService) : IEndpoints
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/cart", async (ClaimsPrincipal claims, [FromBody] object? empty) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var existingCart = await cartService.GetCartByUserIdAsync(userId);
            if (existingCart is not null) return Results.Conflict();

            var cart = await cartService.AddCartAsync(new CreateCartDto { UserId = userId });
            return Results.Created($"/cart/{cart.Id}", cart);

        }).RequireAuthorization();

        app.MapGet("/cart/cartitems/", async (ClaimsPrincipal claims) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var cartId = await cartService.GetCartIdByUserIdAsync(userId);
            if (cartId == 0) return Results.NotFound();

            var cart = await cartService.GetCartItemsByCartIdAsync(cartId);
            return Results.Ok(cart);

        }).RequireAuthorization();

        app.MapGet("/cart", async (ClaimsPrincipal claims) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var cartId = await cartService.GetCartIdByUserIdAsync(userId);
            return Results.Ok(new { CartId = cartId });
        }).RequireAuthorization();

        app.MapPut("/cart", async (ClaimsPrincipal claims, List<CartItemDto> cartItems) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var cart = await cartService.UpdateCartItemsAsync(userId, cartItems);
            return cart == null ? Results.NotFound() : Results.Ok(cart);

        }).RequireAuthorization();

        app.MapDelete("/cart", async (ClaimsPrincipal claims) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();
                
            var cart = await cartService.DeleteCartAsync(userId);
            return cart == null ? Results.NotFound() : Results.Ok();

        }).RequireAuthorization();

    }
}