using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface IOrderService
    {
        public Task<List<OrderDto>> GetOrders();
        public Task<OrderDto> GetOrder(int orderId);
        public Task<OrderDto> PlaceOrder();
    }
}
