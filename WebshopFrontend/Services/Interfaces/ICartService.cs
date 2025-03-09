using Microsoft.AspNetCore.Components.Authorization;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetUserCart();
        Task<CartDto?> SetUserCart();
        Task<CartItemDto> AddItem(int productId, int quantity);
        Task<List<CartItemDto>> GetLocalStorageCartItems();
        Task<List<CartItemDto>> GetUserCartItems();
        Task ClearUserCart();
        Task ClearLocalStorageCart();
        Task UpdateLocalStorageCart(List<CartItemDto> cartItems);
        Task UpdateUserCart(List<CartItemDto> cartItems);
        Task Login();
        Task Logout();
    }
}
