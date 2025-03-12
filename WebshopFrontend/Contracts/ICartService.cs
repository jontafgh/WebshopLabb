﻿using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCart();
        Task<CartDto> SetCart();
        Task<CartItemDto> AddItem(ProductDto product, int quantity, string cartId);
        Task ClearCart();
        Task UpdateCart(List<CartItemDto> cartItems);
    }
}
