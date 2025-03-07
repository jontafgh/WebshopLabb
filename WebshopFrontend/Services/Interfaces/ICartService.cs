using Microsoft.AspNetCore.Components.Authorization;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface ICartService
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        public bool Authenticated { get; set; }
        Task CreateCart();
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate);
        Task<CartItemDto> RemoveItem(int itemId);
        Task<CartItemDto> GetItem(int itemId);
        Task<List<CartItemDto>> GetAll();
        Task ClearCart();
        Task<int> GetCartId();
        Task<CartItemDto> GetCartItemByProductAndCart(int cartId, int productId);
    }
}
