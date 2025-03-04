using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartItemDto> AddItem(int productId);
        Task<CartItemDto> UpdateItem(int itemId, int quantity);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<CartItemDto> GetItem(int itemId);
        Task<CartItemDto[]> GetAll();
        Task ClearCart();
    }
}
