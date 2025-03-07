using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;
using static System.Net.WebRequestMethods;

namespace WebshopFrontend.Services
{
    public class LocalStorageCartService(IJSRuntime js, IHttpClientFactory httpClientFactory) : ICartService
    {
        private readonly IJSRuntime _js = js;
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public int CartId { get; set; }
        private string _cartName = string.Empty;

        public async Task CreateCart(string userId)
        {
            CartId = 1;
            _cartName = $"{userId}-cart{CartId}";
            await _js.InvokeVoidAsync("localStorage.setItem", $"{_cartName}", "[]");
        }

        public async Task<bool> CartExists(string userId)
        {
            _cartName = $"{userId}-cart{CartId}";
            return await _js.InvokeAsync<bool>("GetIfKeyExistsInLocalStorage", _cartName);
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

        public async Task<List<CartItemDto>> GetAll(int cartId)
        {
           return await _js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
        }

        public async Task ClearCart(int cartId)
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
                CartId = CartId,
                Name = product.Name,
                ArtNr = product.ArtNr,
                Quantity = cartItemToAdd.Quantity,
                Price = product.Price.Discount?.DiscountPrice ?? product.Price.Regular
            };
        }
    }
}
