using WebshopFrontend.Contracts;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class UserCartService : ICartService
    {
        public Task<List<CartItemDto>> GetCart()
        {
            throw new NotImplementedException();
        }

        public Task SetCart()
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto> AddItem(int productId, int quantity, string cartId)
        {
            throw new NotImplementedException();
        }

        public Task ClearCart()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCart(List<CartItemDto> cartItems)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> GetUserCart()
        {
            throw new NotImplementedException();
        }

        public Task<CartDto?> SetUserCart()
        {
            throw new NotImplementedException();
        }

        public Task ClearUserCart()
        {
            throw new NotImplementedException();
        }

        public Task<List<CartItemDto>> GetUserCartItems()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserCart(List<CartItemDto> cartItems)
        {
            throw new NotImplementedException();
        }

        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
