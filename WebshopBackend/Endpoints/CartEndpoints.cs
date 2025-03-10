﻿using System.Security.Claims;
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

            var existingCart = await cartService.GetCartAsync(userId);
            if (existingCart is not null) return Results.Conflict();

            var cart = await cartService.CreateCartAsync(new CreateCartDto { Id = userId });
            return Results.Created($"/cart/{cart.Id}", cart);

        }).RequireAuthorization();

        app.MapGet("/cart/cartitems", async (ClaimsPrincipal claims) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var cart = await cartService.GetCartAsync(userId);
            if (cart == null) return Results.NotFound();

            var cartitems = await cartService.GetCartItemsAsync(userId);
            return Results.Ok(cartitems);

        }).RequireAuthorization();

        app.MapGet("/cart", async (ClaimsPrincipal claims) =>
        {
            var userId = userService.GetUserId(claims);
            if (userId is null) return Results.Unauthorized();

            var existingCart = await cartService.GetCartAsync(userId);
            return existingCart is not null ? Results.Ok(existingCart) : Results.NotFound();

        }).RequireAuthorization();

        app.MapPut("/cart/cartitems", async (ClaimsPrincipal claims, List<CartItemDto> cartItems) =>
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