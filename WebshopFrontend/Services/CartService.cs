using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CartService(IHttpClientFactory httpClientFactory, IJSRuntime js, ICounterService counterService) : ICartService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        private bool _loggedIn = false;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task SetUserCart()
        {
            var result = await _httpClient.PostAsJsonAsync("/cart", new CreateCartDto { UserId = UserId });
            if (result.IsSuccessStatusCode)
            {
                var cartJson = await result.Content.ReadAsStringAsync();
                var cart = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions);

                if (cart != null) 
                    CartId = cart.Id;
            }
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
            var cartItem = await GetProduct(cartItemToAdd);
            await js.InvokeVoidAsync("AddItemToLocalStorage", cartItem);
            return cartItem;
        }

        public async Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate)
        {
            return await js.InvokeAsync<CartItemDto>("UpdateItemInLocalStorage", cartItemToUpdate);
        }

        public async Task<CartItemDto> RemoveItem(int itemId)
        {
            return await js.InvokeAsync<CartItemDto>("RemoveItemFromLocalStorage", itemId);
        }

        public async Task<List<CartItemDto>> GetAllItems()
        {
            var cartItems = await js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");

            if (_loggedIn)
                await SetUserCart(cartItems);

            return cartItems;
        }

        public void SetUser(int cartId, string userId)
        {
            CartId = cartId;
            UserId = userId;
            _loggedIn = true;
        }

        public async Task Login(int cartId, string userId)
        {
            SetUser(cartId, userId);

            if (CartId == 0) 
                await SetUserCart();

            await SetLocalStorageCart();
            var cart = await GetUserCart();

            counterService.SetCount(cart.Sum(ci => ci.Quantity));
            _loggedIn = true;
        }
        public async Task Logout()
        {
            await GetAllItems();
            await ClearLocalStorageCart();

            counterService.SetCount(0);
            _loggedIn = false;
        }

        public async Task ClearLocalStorageCart()
        {
            await js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public async Task<List<CartItemDto>> GetUserCart()
        {
            var result = await _httpClient.GetAsync($"/cart/{CartId}/cartitems");
            var content = await result.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<List<CartItemDto>>(content, _jsonSerializerOptions) ?? [];
        }

        public async Task SetUserCart(List<CartItemDto> cartItems)
        {
            var json = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync($"/cart/{CartId}", content);
        }

        public async Task SetLocalStorageCart()
        {
            var result = await _httpClient.GetAsync($"/cart/{CartId}/cartitems");

            if (!result.IsSuccessStatusCode) 
                return;

            var cartItemsJson = await result.Content.ReadAsStringAsync();
            await js.InvokeVoidAsync("localStorage.setItem", "cart", cartItemsJson);
        }

        public async Task<CartItemDto> GetProduct(CartItemToAddDto cartItemToAdd)
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
