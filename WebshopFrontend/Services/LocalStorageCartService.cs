using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;
using static System.Net.WebRequestMethods;

namespace WebshopFrontend.Services
{
    public class LocalStorageCartService(IJSRuntime js, HttpClient httpClient) : ICartService
    {
        private readonly IJSRuntime _js = js;
        private readonly HttpClient _httpClient = httpClient;

        public async Task CreateCart(int userId)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "cart", "[]");
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAdd)
        {
            await _js.InvokeVoidAsync("AddItemToLocalStorage", cartItemToAdd);
            return await GetCartItemDto(cartItemToAdd);
        }

        public async Task<CartItemDto> UpdateItem(CartItemToUpdateDto cartItemToUpdate)
        {
           var cartItem = await _js.InvokeAsync<CartItemToAddDto>("UpdateItemInLocalStorage", cartItemToUpdate);
           return await GetCartItemDto(cartItem);
        }

        public async Task<CartItemDto> RemoveItem(int itemId)
        {
            var removedItem = await _js.InvokeAsync<CartItemToAddDto>("RemoveItemFromLocalStorage", itemId);
            return await GetCartItemDto(removedItem);
        }

        public async Task<CartItemDto> GetItem(int itemId)
        {
            var cartItem = await _js.InvokeAsync<CartItemToAddDto>("GetItemFromLocalStorage", itemId);
            return await GetCartItemDto(cartItem);
        }

        public async Task<List<CartItemDto>> GetAll(int userId)
        {
            var cartItems = await _js.InvokeAsync<List<CartItemToAddDto>>("GetCartFromLocalStorage");
            var cartItemDtos = new List<CartItemDto>();
            foreach (var cartItem in cartItems)
            {
                cartItemDtos.Add(await GetCartItemDto(cartItem));
            }
            return cartItemDtos.ToList();
        }

        public async Task ClearCart(int userId)
        {
            await _js.InvokeVoidAsync("RemoveCartFromLocalStorage");
        }

        public async Task<CartItemDto> GetCartItemDto(CartItemToAddDto cartItemToAdd)
        {
            var product = await _httpClient.GetFromJsonAsync<IProduct>($"/products/{cartItemToAdd.ProductId}");

            return new CartItemDto
            {
                ProductId = product!.Id,
                CartId = cartItemToAdd.CartId,
                Name = product.Name,
                ArtNr = product.ArtNr,
                Quantity = cartItemToAdd.Quantity
            };
        }
    }
}
