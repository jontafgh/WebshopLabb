﻿namespace WebshopBackend.Models
{
    public class Cart
    {
        public string Id { get; set; } = null!;
        public WebshopUser User { get; set; } = null!;
        public List<CartItem> CartItems { get; set; } = [];
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string CartId { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
    }
}
