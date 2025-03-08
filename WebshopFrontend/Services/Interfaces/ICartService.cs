using Microsoft.AspNetCore.Components.Authorization;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<List<CartItemDto>> GetAllItems();
        void SetUser(int cartId, string userId);
        Task SetUserCart();
        Task SetLocalStorageCart();
        Task ClearLocalStorageCart();
        Task Login(int cartId, string userId);
        Task Logout();
    }
}
