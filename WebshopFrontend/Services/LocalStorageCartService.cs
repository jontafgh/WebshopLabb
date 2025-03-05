using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;
using static System.Net.WebRequestMethods;

namespace WebshopFrontend.Services
{
    public class LocalStorageCartService(IJSRuntime js, HttpClient httpClient) : ICartService
    {
        private readonly IJSRuntime _js = js;
        private readonly HttpClient _httpClient = httpClient;

        public async Task CreateCart(int userId)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "cart", "[]");
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
            var cartItem = await GetCartItemDto(cartItemToAdd);
            await _js.InvokeVoidAsync("AddItemToLocalStorage", cartItem);
            return cartItem;
        }

        public async Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate)
        {
           return await _js.InvokeAsync<CartItemDto>("UpdateItemInLocalStorage", cartItemToUpdate);
        }

        public async Task<CartItemDto> RemoveItem(int itemId)
        {
            return await _js.InvokeAsync<CartItemDto>("RemoveItemFromLocalStorage", itemId);
        }

        public async Task<CartItemDto> GetItem(int itemId)
        {
            return await _js.InvokeAsync<CartItemDto>("GetItemFromLocalStorage", itemId);
        }

        public async Task<List<CartItemDto>> GetAll(int userId)
        {
           return await _js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
        }

        public async Task ClearCart(int userId)
        {
            await _js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public async Task<CartItemDto> GetCartItemDto(CartItemToAddDto cartItemToAdd)
        {
            var id = cartItemToAdd.ProductId;
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"/products/{id}");

            return new CartItemDto
            {
                Id = product!.Id,
                ProductId = product!.Id,
                CartId = cartItemToAdd.CartId,
                Name = product.Name,
                ArtNr = product.ArtNr,
                Quantity = cartItemToAdd.Quantity,
                Price = product.Price.Discount?.DiscountPrice ?? product.Price.Regular
            };
        }
    }
}
