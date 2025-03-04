using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderDto> GetOrder(int id);
        public Task<List<OrderDto>> GetOrders(int userId);
        public Task<OrderDto> AddOrder(OrderDto order);
    }
}
