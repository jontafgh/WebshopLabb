using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderDto>> GetOrders();
        public Task<OrderDto> GetOrder(int orderId);
        public Task<OrderDto> PlaceOrder();
    }
}
