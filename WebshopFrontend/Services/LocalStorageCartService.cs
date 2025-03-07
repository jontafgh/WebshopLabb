using Microsoft.AspNetCore.Components.Authorization;
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
        public string UserId { get; set; } = string.Empty;
        public bool Authenticated { get; set; }
        private string _cartName = string.Empty;

        public async Task CreateCart()
        {
            CartId = 1;
            _cartName = $"-cart{CartId}";
            await _js.InvokeVoidAsync("localStorage.setItem", $"{_cartName}", "[]");
        }

        public async Task<int> GetCartId()
        {
            _cartName = $"-cart{CartId}";
            return await _js.InvokeAsync<int>("GetIfKeyExistsInLocalStorage", _cartName);
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

        public async Task<List<CartItemDto>> GetAll()
        {
           return await _js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
        }

        public async Task ClearCart()
        {
            await _js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public Task<CartItemDto> GetCartItemByProductAndCart(int cartId, int productId)
        {
            throw new System.NotImplementedException();
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
