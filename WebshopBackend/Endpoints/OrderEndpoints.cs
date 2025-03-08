using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Endpoints
{
    public class OrderEndpoints(IOrderService orderService, IUserService userService) : IEndpoints
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapPost("/order", async (ClaimsPrincipal claims, PlaceOrderDto placeOrderDto) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId is null) return Results.Unauthorized();

                var order = await orderService.AddOrderAsync(userId, placeOrderDto);
                return Results.Created($"/order/{order.Id}", order);

            }).RequireAuthorization();

            app.MapGet("/order/{orderId:int}", async (int orderId, ClaimsPrincipal claims) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId is null) return Results.Unauthorized();

                var order = await orderService.GetOrderAsync(orderId, userId);
                return order == null ? Results.NotFound() : Results.Ok(order);

            }).RequireAuthorization();

            app.MapGet("/orders", async (ClaimsPrincipal claims) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId is null) return Results.Unauthorized();

                var orders = await orderService.GetOrdersAsync(userId);
                return Results.Ok(orders);

            }).RequireAuthorization();
        }
    }
}
