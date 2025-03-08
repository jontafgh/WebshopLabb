using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class CartService(WebshopContext dbContext) : ICartService
    {
        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> AddCartAsync(CreateCartDto createCartDto)
        {
            var cart = createCartDto.ToCart();
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<int> GetCartIdByUserIdAsync(string userId)
        {
            return await dbContext.Carts.Where(c => c.UserId == userId)
                .AsNoTracking()
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CartItemDto>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await dbContext.CartItems.Where(ci => ci.CartId == cartId)
                .AsNoTracking()
                .Include(p => p.Product)
                .ThenInclude(p => p.Price)
                .ThenInclude(d => d.Discount)
                .Select(c => c.ToCartItemDto())
                .ToListAsync();
        }

        public async Task<Cart?> UpdateCartItemsAsync(string userId, List<CartItemDto> cartItems)
        {
            var cart = await dbContext.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null) return cart;
            
            cart.CartItems = cartItems.Select(ci => ci.ToCartItem()).ToList();
            await dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> DeleteCartAsync(string userId)
        {
            var cart = await dbContext.Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null) return null;

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync();
            return cart;
        }
    }
}
