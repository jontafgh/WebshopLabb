using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class MockOrderService : IOrderService
    {
        private OrderDto Order { get; set; }
        public async Task<OrderDto> GetOrder(int id)
        {
            return Order;
        }

        public Task<List<OrderDto>> GetOrders(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> AddOrder(OrderDto order)
        {
            Order = order;
            return Task.FromResult(Order);
        }
    }
}
