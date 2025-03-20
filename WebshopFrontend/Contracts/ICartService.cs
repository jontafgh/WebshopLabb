using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCart();
        Task<CartItemDto> AddItem(ProductDto product, int quantity, string cartId);
        Task ClearCart();
        Task SetCart(List<CartItemDto> cartItems);
    }
}
