using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class OrderService(WebshopContext dbContext) : IOrderService
    {
        public async Task<OrderDto> AddOrderAsync(string userId, PlaceOrderDto placeOrderDto)
        {
            var order = placeOrderDto.ToOrder(userId);
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order.ToOrderDto();
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId, string userId)
        {
            return await dbContext.Orders.Where(o => o.Id == orderId && o.UserId == userId)
                .AsNoTracking()
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ThenInclude(p => p.Price)
                .ThenInclude(d => d.Discount)
                .Select(o => o.ToOrderDtoWithCartItems())
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            return await dbContext.Orders
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ThenInclude(p => p.Price)
                .ThenInclude(d => d.Discount)
                .Select(o => o.ToOrderDtoWithCartItems())
                .ToListAsync();
        }
    }
}
