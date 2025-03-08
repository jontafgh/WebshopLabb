using Microsoft.AspNetCore.Components.Authorization;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        public int CartId { get; set; }
        Task<List<CartItemDto>> GetAllItems();
        Task SetUserCart();
        Task ClearUserCart();
        Task ClearLocalStorageCart();
        Task<CartItemDto> AddItem(int productId, int quantity);
        Task Login();
        Task Logout();
        Task UpdateLocalStorageCart(List<CartItemDto> cartItems);
        Task UpdateUserCart(List<CartItemDto> cartItems);
    }
}
