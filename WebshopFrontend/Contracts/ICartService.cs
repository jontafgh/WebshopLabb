using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCart();
        Task SetCart();
        Task<CartItemDto> AddItem(int productId, int quantity, string cartId);
        Task ClearCart();
        Task UpdateCart(List<CartItemDto> cartItems);
       

        Task<CartDto> GetUserCart();
        Task<CartDto?> SetUserCart();
        Task ClearUserCart();
        Task<List<CartItemDto>> GetUserCartItems();
        Task UpdateUserCart(List<CartItemDto> cartItems);

        Task Login();
        Task Logout();
    }
}
