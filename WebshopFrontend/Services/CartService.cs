using Microsoft.JSInterop;
using System.Runtime.CompilerServices;
using System.Text.Json;
using WebshopFrontend.Services.Identity;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CartService(IHttpClientFactory httpClientFactory, IUserService userService) : ICartService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public int CartId { get; set; }
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task CreateCart(string userId)
        {
            var result = await _httpClient.PostAsJsonAsync("/cart", new CreateCartDto { UserId = userId });
            if (result.IsSuccessStatusCode)
            {
                var cartJson = await result.Content.ReadAsStringAsync();
                var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);
                if (cart != null) CartId = cart.Id;
            }
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
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

        public async Task<List<CartItemDto>> GetAll(int cartId)
        {
            var result = await _httpClient.GetAsync($"/cart/{cartId}/cartitems");
            if (!result.IsSuccessStatusCode) return [];
            var cartItemsJson = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CartItemDto>>(cartItemsJson, _jsonSerializerOptions) ?? [];
        }

        public async Task ClearCart(int cartId)
        {
            var result = await _httpClient.DeleteAsync($"/cart/{cartId}");
        }

        public Task<bool> CartExists(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
