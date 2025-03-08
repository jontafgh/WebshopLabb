using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CartService(IHttpClientFactory httpClientFactory, IJSRuntime js, ICounterService counterService, IProductService productService) : ICartService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public int CartId { get; set; }
        
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task Login()
        {
            if (CartId == 0) await SetUserCart();

            var cart = await GetUserCart();
            await UpdateLocalStorageCart(cart);

            counterService.SetCount(cart.Sum(ci => ci.Quantity));
        }

        public async Task Logout()
        {
            await ClearLocalStorageCart();
            counterService.SetCount(0);
        }

        public async Task<CartItemDto> AddItem(int productId, int quantity)
        {
            var cartItem = await GetCartItem(productId, quantity);
            await js.InvokeVoidAsync("AddItemToLocalStorage", cartItem);
            return cartItem;
        }

        public async Task<List<CartItemDto>> GetAllItems()
        {
            return await js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
        }

        public async Task UpdateLocalStorageCart(List<CartItemDto> cartItems)
        {
            var cartItemsJson = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            await js.InvokeVoidAsync("localStorage.setItem", "cart", cartItemsJson);
        }

        public async Task UpdateUserCart(List<CartItemDto> cartItems)
        {
            var json = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync($"/cart", content);
        }

        public async Task SetUserCart()
        {
            const string empty = "{}";
            var emptyContent = new StringContent(empty, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsJsonAsync("/cart", emptyContent);
            if (!result.IsSuccessStatusCode) return;

            var cartJson = await result.Content.ReadAsStringAsync();
            var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);

            if (cart != null) CartId = cart.Id;
        }

        public async Task<List<CartItemDto>> GetUserCart()
        {
            var result = await _httpClient.GetAsync($"/cart/cartitems");
            var content = await result.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<List<CartItemDto>>(content, _jsonSerializerOptions) ?? [];
        }
        public async Task ClearLocalStorageCart()
        {
            await js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public async Task ClearUserCart()
        {
            await _httpClient.DeleteAsync($"/cart");
        }

        private async Task<CartItemDto> GetCartItem(int productId, int quantity)
        {
            var product = await productService.GetProductById(productId);

            return new CartItemDto
            {
                Id = product.Id,
                ProductId = product.Id,
                CartId = CartId,
                Name = product.Name,
                ArtNr = product.ArtNr,
                Quantity = quantity,
                Price = product.Price.Discount?.DiscountPrice ?? product.Price.Regular
            };
        }

        
    }
}
