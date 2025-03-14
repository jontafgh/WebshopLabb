using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class CartService(IDbContextFactory<WebshopContext> dbContextFactory) : ICartService
    {
        public async Task<Cart?> GetCartAsync(string id) 
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            return await dbContext.Carts.FindAsync(id);
        }

        public async Task<Cart> CreateCartAsync(CreateCartDto createCartDto)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cart = createCartDto.ToCart();
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();
            return cart;
        }
        
        public async Task<List<CartItemDto>> GetCartItemsAsync(string cartId)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            return await dbContext.CartItems.AsNoTracking().Where(ci => ci.CartId == cartId)
                .Include(p => p.Product)
                .ThenInclude(p => p.Price!)
                .ThenInclude(d => d.Discount)
                .Select(c => c.ToCartItemDto())
                .ToListAsync();
        }

        public async Task<Cart?> UpdateCartItemsAsync(string id, List<CartItemDto> cartItems)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var cart = await dbContext.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cart is null) return cart;
            
            cart.CartItems = cartItems.Select(ci => ci.ToCartItem()).ToList();
            await dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> DeleteCartAsync(string id)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var cart = await dbContext.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cart is null) return null;

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync();
            return cart;
        }
    }
}
