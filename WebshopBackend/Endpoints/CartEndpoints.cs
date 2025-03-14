using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Endpoints;

public class CartEndpoints(ICartService cartService) : IEndpoints
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/cart", async (ClaimsPrincipal claims) =>
        {
            var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

            var cart = await cartService.GetCartAsync(userId);
            return cart is not null ? Results.Ok(cart) : Results.NotFound();

        }).RequireAuthorization();

        app.MapPut("/cart", async (ClaimsPrincipal claims, List<CartItemDto> cartItems) =>
        {
            var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

            var cart = await cartService.UpdateCartAsync(userId, cartItems);
            return cart == null ? Results.NotFound() : Results.Ok(cart);

        }).RequireAuthorization();

        app.MapDelete("/cart", async (ClaimsPrincipal claims) =>
        {
            var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

            var cart = await cartService.DeleteCartAsync(userId);
            return cart == null ? Results.NotFound() : Results.Ok();

        }).RequireAuthorization();

    }
}