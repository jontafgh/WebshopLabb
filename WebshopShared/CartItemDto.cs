namespace WebshopShared
{
    public class CreateCartDto
    {
        public string Id { get; set; } = null!;
    }
    public class CartDto
    {
        public string Id { get; set; } = null!;
        public List<CartItemDto> CartItems { get; set; } = [];
    }
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CartId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ArtNr { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
