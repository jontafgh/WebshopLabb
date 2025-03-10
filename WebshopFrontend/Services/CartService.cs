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
        
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task Login()
        {
            var cart = await GetUserCart();
            if (string.IsNullOrWhiteSpace(cart.Id)) 
                cart = await SetUserCart();

            cart!.CartItems = await GetUserCartItems();
            await UpdateCart(cart.CartItems);
            counterService.SetCount(cart.CartItems.Sum(ci => ci.Quantity));
        }
        public async Task Logout()
        {
            var cart = await GetCart();
            await UpdateUserCart(cart);
            await ClearCart();
            counterService.SetCount(0);
        }
        public async Task<CartItemDto> AddItem(int productId, int quantity, string cartId)
        {
            var cartItem = await GetCartItem(productId, quantity, cartId);
            await js.InvokeVoidAsync("AddItemToLocalStorage", cartItem);
            return cartItem;
        }
        public async Task<List<CartItemDto>> GetCart()
        {
            var cart = await js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
            return cart;
        }
        public async Task UpdateCart(List<CartItemDto> cartItems)
        {
            var cartItemsJson = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            await js.InvokeVoidAsync("localStorage.setItem", "cart", cartItemsJson);
        }
        public async Task SetCart()
        {
            await js.InvokeVoidAsync("localStorage.setItem", "cart", "[]");
        }
        public async Task ClearCart()
        {
            await js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        private async Task<CartItemDto> GetCartItem(int productId, int quantity, string cartId)
        {
            var product = await productService.GetProductById(productId);

            return new CartItemDto
            {
                Id = product.Id,
                ProductId = product.Id,
                CartId = cartId,
                Name = product.Name,
                ArtNr = product.ArtNr,
                Quantity = quantity,
                Price = product.Price.Discount?.DiscountPrice ?? product.Price.Regular
            };
        }
        public async Task<CartDto> GetUserCart()
        {
            var result = await _httpClient.GetAsync("/cart");
            if (!result.IsSuccessStatusCode) return new CartDto();

            var cartJson = await result.Content.ReadAsStringAsync();
            var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);
            return cart ?? new CartDto();
        }
        public async Task UpdateUserCart(List<CartItemDto> cartItems)
        {
            var json = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"/cart/cartitems", content);
        }
        public async Task<CartDto?> SetUserCart()
        {
            const string empty = "{}";
            var emptyContent = new StringContent(empty, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsJsonAsync("/cart", emptyContent);
            if (!result.IsSuccessStatusCode) return null;

            var cartJson = await result.Content.ReadAsStringAsync();
            var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);

            return cart ?? null;
        }
        public async Task ClearUserCart()
        {
            var emptyCollection = new List<CartItemDto>();
            await UpdateUserCart(emptyCollection);
        }
        public async Task<List<CartItemDto>> GetUserCartItems()
        {
            var result = await _httpClient.GetAsync($"/cart/cartitems");
            var content = await result.Content.ReadAsStreamAsync();
            var cartitems = JsonSerializer.Deserialize<List<CartItemDto>>(content, _jsonSerializerOptions) ?? [];
            return cartitems;
        }
    }
}
