using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        Task CreateCart(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<CartItemDto> GetItem(int itemId);
        Task<List<CartItemDto>> GetAll(int userId);
        Task ClearCart(int userId);
    }
}
