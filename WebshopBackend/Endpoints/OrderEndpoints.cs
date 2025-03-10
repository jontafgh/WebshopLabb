using System.Security.Claims;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Endpoints
{
    public class OrderEndpoints(IOrderService orderService) : IEndpoints
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapPost("/order", async (ClaimsPrincipal claims, PlaceOrderDto placeOrderDto) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

                var order = await orderService.AddOrderAsync(userId, placeOrderDto);
                return Results.Created($"/order/{order.Id}", order);

            }).RequireAuthorization();

            app.MapGet("/order/{orderId:int}", async (int orderId, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

                var order = await orderService.GetOrderAsync(orderId, userId);
                return order == null ? Results.NotFound() : Results.Ok(order);

            }).RequireAuthorization();

            app.MapGet("/orders", async (ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

                var orders = await orderService.GetOrdersAsync(userId);
                return Results.Ok(orders);

            }).RequireAuthorization();
        }
    }
}
