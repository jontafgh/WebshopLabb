namespace WebshopBackend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public WebshopUser User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderLine> OrderLines { get; set; } = [];
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
