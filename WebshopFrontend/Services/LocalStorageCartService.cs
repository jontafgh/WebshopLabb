using Microsoft.JSInterop;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class LocalStorageCartService(IJSRuntime js) : ICartService
    {
        private readonly IJSRuntime _js = js;

        public async Task<CartItemDto> AddItem(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto> UpdateItem(int itemId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto> RemoveItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto> GetItem(int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task ClearCart()
        {
            throw new NotImplementedException();
        }
    }
}
