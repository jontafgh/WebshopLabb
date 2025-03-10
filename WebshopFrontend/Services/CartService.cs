using Microsoft.JSInterop;
using System.Text.Json;
using WebshopFrontend.Contracts;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CartService(IJSRuntime js, IProductService productService) : ICartService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        
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
        public async Task<CartDto> SetCart()
        {
            await js.InvokeVoidAsync("localStorage.setItem", "cart", "[]");
            return new CartDto { CartItems = [] };
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
        
    }
}
