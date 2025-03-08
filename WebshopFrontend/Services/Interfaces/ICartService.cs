using Microsoft.AspNetCore.Components.Authorization;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        public int CartId { get; set; }
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<List<CartItemDto>> GetAllItems();
        Task SetUserCart();
        Task UpdateUserCart(List<CartItemDto> cartItems);
        Task SetLocalStorageCart();
        Task ClearUserCart();
        Task ClearLocalStorageCart();
        Task Login();
        Task Logout();
    }
}
