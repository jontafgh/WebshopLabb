using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using WebshopFrontend.Services.Identity;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CartService(IHttpClientFactory httpClientFactory) : ICartService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool Authenticated { get; set; }

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task CreateCart()
        {
            if (Authenticated && CartId == 0)
            {
                CartId = await GetCartId();
                if (CartId == 0)
                {
                    var result = await _httpClient.PostAsJsonAsync("/cart", new CreateCartDto { UserId = UserId });
                    if (result.IsSuccessStatusCode)
                    {
                        var cartJson = await result.Content.ReadAsStringAsync();
                        var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);
                        if (cart != null) CartId = cart.Id;
                    }
                }
            }
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
            await CreateCart();

            if (!Authenticated) return new CartItemDto();
            cartItemToAdd.CartId = CartId;

            var existingItem = await GetCartItemByProductAndCart(cartItemToAdd.CartId, cartItemToAdd.ProductId);
            if (existingItem.Id != 0)
            {
                var updatedExistingItem = await UpdateItem(new CartItemToUpdateDto { Id = existingItem.Id, Quantity = existingItem.Quantity + 1 });
                return updatedExistingItem;
            }

            var result = await _httpClient.PostAsJsonAsync("/cart/cartitem", cartItemToAdd);
            if (!result.IsSuccessStatusCode) return new CartItemDto();

            var cartItemJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartItemDto>(cartItemJson, _jsonSerializerOptions) ?? new CartItemDto();
        }

        public async Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate)
        {
            var result = await _httpClient.PutAsJsonAsync("/cart/cartitem", cartItemToUpdate);
            if (!result.IsSuccessStatusCode) return new CartItemDto();
            var cartItemJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartItemDto>(cartItemJson, _jsonSerializerOptions) ?? new CartItemDto();
        }

        public async Task<CartItemDto> RemoveItem(int itemId)
        {
            var result = await _httpClient.DeleteAsync($"/cart/cartitem/{itemId}");
            if (!result.IsSuccessStatusCode) return new CartItemDto();
            var cartItemJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartItemDto>(cartItemJson, _jsonSerializerOptions) ?? new CartItemDto();
        }

        public async Task<CartItemDto> GetItem(int itemId)
        {
            var result = await _httpClient.GetAsync($"/cart/cartitem/{itemId}");
            if (!result.IsSuccessStatusCode) return new CartItemDto();
            var cartItemJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartItemDto>(cartItemJson, _jsonSerializerOptions) ?? new CartItemDto();

        }

        public async Task<List<CartItemDto>> GetAll()
        {
            if (CartId == 0 && Authenticated)
            {
                CartId = await GetCartId();
            }
            var result = await _httpClient.GetAsync($"/cart/{CartId}/cartitems");
            if (!result.IsSuccessStatusCode) return [];
            var cartItemsJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CartItemDto>>(cartItemsJson, _jsonSerializerOptions) ?? [];
        }

        public async Task ClearCart()
        {
            var result = await _httpClient.DeleteAsync($"/cart/{UserId}");
        }

        public async Task<int> GetCartId()
        {
            var result = await _httpClient.GetAsync($"/cart/{UserId}");
            if (!result.IsSuccessStatusCode) return 0;
            var resultJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(resultJson, _jsonSerializerOptions);
        }

        public async Task<CartItemDto> GetCartItemByProductAndCart(int cartId, int productId)
        {
            var result = await _httpClient.GetAsync($"/cart/{cartId}/product/{productId}/");
            if (!result.IsSuccessStatusCode) return new CartItemDto();
            var cartItemJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartItemDto>(cartItemJson, _jsonSerializerOptions) ?? new CartItemDto();
        }
    }
}
