using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class CartService(IDbContextFactory<WebshopContext> dbContextFactory) : ICartService
    {
        public async Task<CartDto?> GetCartAsync(string id) 
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cart = await dbContext.Carts.FindAsync(id);

            if (cart is null)
            {
                dbContext.Carts.Add(new Cart { Id = id, CartItems = [] });
                await dbContext.SaveChangesAsync();
                return new CartDto { Id = id, CartItems = [] };
            }

            var cartDto = new CartDto
            {
                Id = id,
                CartItems = await dbContext.CartItems.AsNoTracking().Where(ci => ci.CartId == cart.Id)
                    .Include(p => p.Product)
                    .ThenInclude(p => p.Price!)
                    .ThenInclude(d => d.Discount)
                    .Select(c => c.ToCartItemDto())
                    .ToListAsync()
            };

            return cartDto;
        }
        
        public async Task<CartDto?> UpdateCartAsync(string id, List<CartItemDto> cartItems)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cart = await dbContext.Carts.FindAsync(id);
            if (cart is null) return null;

            cart.CartItems = cartItems.Select(ci => ci.ToCartItem()).ToList();
            await dbContext.SaveChangesAsync();

            return new CartDto { Id = id, CartItems = cartItems };
        }

        public async Task<CartDto?> DeleteCartAsync(string id)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var cart = await dbContext.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cart is null) return null;

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync();
            return new CartDto {Id = cart.Id };
        }
    }
}
