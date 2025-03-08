using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface ICartService
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<Cart> AddCartAsync(CreateCartDto createCartDto);
        Task<int> GetCartIdByUserIdAsync(string userId);
        Task<List<CartItemDto>> GetCartItemsByCartIdAsync(int cartId);
        Task<Cart?> UpdateCartItemsAsync(string userId, List<CartItemDto> cartItems);
        Task<Cart?> DeleteCartAsync(string userId);
    }
}
