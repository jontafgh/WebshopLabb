using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class OrderService(IDbContextFactory<WebshopContext> dbContextFactory) : IOrderService
    {
        public async Task<OrderDto> AddOrderAsync(string userId, PlaceOrderDto placeOrderDto)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var order = placeOrderDto.ToOrder(userId);
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order.ToOrderDtoWithCartItems(placeOrderDto.CartItems);
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId, string userId)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Orders.Where(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ThenInclude(p => p.Price!)
                .ThenInclude(d => d.Discount)
                .Select(o => o.ToOrderDtoWithCartItems())
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ThenInclude(p => p.Price!)
                .ThenInclude(d => d.Discount)
                .Select(o => o.ToOrderDtoWithCartItems())
                .ToListAsync();
        }
    }
}
