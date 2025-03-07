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
        private readonly IJSRuntime _js = js;
        private readonly ICounterService _counterService = counterService;
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool Authenticated { get; set; }

        private const string LocalCartName = "cart";

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task CreateCart()
        {
            if (Authenticated && CartId == 0)
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

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
            await CreateCart();
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

        public async Task<List<CartItemDto>> GetAll()
        {
            var cartItems = await _js.InvokeAsync<List<CartItemDto>>("GetCartFromLocalStorage");
            await SetDbCart(cartItems);
            return cartItems;
        }

        public async Task OnLogin()
        {
            await CreateCart();
            await SetLocalStorageCart();
            var dbCart = await GetDbCart();
            _counterService.SetCount(dbCart.Sum(ci => ci.Quantity));
        }
        public async Task OnLogout()
        {
            await GetAll();
            await ClearLocalStorage();
            _counterService.SetCount(0);
        }

        public async Task ClearLocalStorage()
        {
            await _js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public async Task<List<CartItemDto>> GetDbCart()
        {
            if (CartId == 0) return [];
            var result = await _httpClient.GetAsync($"/cart/{CartId}/cartitems");
            var content = await result.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<List<CartItemDto>>(content, _jsonSerializerOptions) ?? [];
        }
        
        public async Task SetDbCart(List<CartItemDto> cartItems)
        {
            if (CartId == 0) return;
            var json = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync($"/cart/{CartId}", content);
        }

        public async Task SetLocalStorageCart()
        {
            if (CartId == 0) return;
            var result = await _httpClient.GetAsync($"/cart/{CartId}/cartitems");
            if (!result.IsSuccessStatusCode) return;
            var cartItemsJson = await result.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("localStorage.setItem", $"{LocalCartName}", cartItemsJson);
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
