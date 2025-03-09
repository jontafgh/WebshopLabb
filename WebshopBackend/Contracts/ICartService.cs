using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface ICartService
    {
        Task<Cart?> GetCartAsync(string id);
        Task<Cart> CreateCartAsync(CreateCartDto createCartDto);
        Task<List<CartItemDto>> GetCartItemsAsync(string cartId);
        Task<Cart?> UpdateCartItemsAsync(string id, List<CartItemDto> cartItems);
        Task<Cart?> DeleteCartAsync(string id);
    }
}
