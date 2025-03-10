using System.Text;
using System.Text.Json;
using WebshopFrontend.Contracts;
using WebshopShared;

namespace WebshopFrontend.Services;

public class ApiCartService(IHttpClientFactory httpClientFactory) : ICartService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task<List<CartItemDto>> GetCart()
    {
        var result = await _httpClient.GetAsync($"/cart/cartitems");
        var content = await result.Content.ReadAsStreamAsync();
        var cartitems = JsonSerializer.Deserialize<List<CartItemDto>>(content, _jsonSerializerOptions) ?? [];
        return cartitems;
    }

    public async Task<CartDto> SetCart()
    {
        const string empty = "{}";
        var emptyContent = new StringContent(empty, Encoding.UTF8, "application/json");
            
        var cart = await _httpClient.PostAsJsonAsync("/cart", emptyContent);
        var cartJson = await cart.Content.ReadAsStringAsync();
        var cartDto = JsonSerializer.Deserialize<CartDto>(cartJson, _jsonSerializerOptions) ?? new CartDto();
        return cartDto;
    }

    public Task<CartItemDto> AddItem(int productId, int quantity, string cartId)
    {
        throw new NotImplementedException();
    }

    public async Task ClearCart()
    {
        var emptyCollection = new List<CartItemDto>();
        await UpdateCart(emptyCollection);
    }

    public async Task UpdateCart(List<CartItemDto> cartItems)
    {
        var json = JsonSerializer.Serialize(cartItems, _jsonSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"/cart/cartitems", content);
    }
}