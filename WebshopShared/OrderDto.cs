namespace WebshopShared
{
    public class PlaceOrderDto
    {
        public List<CartItemDto> CartItems { get; set; } = [];
    }
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CartItemDto> CartItems { get; set; } = [];
        public List<string> Errors { get; set; } = [];
        public bool Valid { get; set; }
    }
}
