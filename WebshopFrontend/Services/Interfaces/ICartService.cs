using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        public int CartId { get; set; }
        Task CreateCart(string userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<CartItemDto> GetItem(int itemId);
        Task<List<CartItemDto>> GetAll(int cartId);
        Task ClearCart(int cartId);
        Task<bool> CartExists(string userId);
    }
}
