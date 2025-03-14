using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(string id);
        Task<CartDto?> UpdateCartAsync(string id, List<CartItemDto> cartItems);
        Task<CartDto?> DeleteCartAsync(string id);
    }
}
