using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class OrderService(IDbContextFactory<WebshopContext> dbContextFactory) : IOrderService
    {
        public async Task<OrderDto> TryUpdateStockAsync(PlaceOrderDto placeOrderDto)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var products = await dbContext.Products.Include(p => p.Stock)
                .Where(p => placeOrderDto.CartItems.Select(ci => ci.ArtNr).Contains(p.ArtNr))
                .ToListAsync();

            var errors = (from product in products
                          let cartItem = placeOrderDto.CartItems.FirstOrDefault(ci => ci.ArtNr == product.ArtNr)!
                          where product.Stock!.Quantity < cartItem.Quantity
                          select $"Not enough stock for {product.Name} {product.ArtNr}, {cartItem.Quantity} in cart {product.Stock.Quantity} available").ToList();

            if (errors.Count != 0) return new OrderDto { Errors = errors, Valid = errors.Count == 0 };

            foreach (var product in products)
            {
                product.Stock!.Quantity -= placeOrderDto.CartItems.First(ci => ci.ArtNr == product.ArtNr).Quantity;
                dbContext.Entry(product).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return new OrderDto { Errors = errors, Valid = errors.Count == 0 };
        }

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

            return await dbContext.Orders.AsNoTracking()
                .Where(o => o.Id == orderId && o.UserId == userId)
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
            return await dbContext.Orders.AsNoTracking()
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
