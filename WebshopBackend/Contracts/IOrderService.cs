using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface IOrderService
    {
        Task<OrderDto> AddOrderAsync(string userId, PlaceOrderDto placeOrderDto);
        Task<OrderDto?> GetOrderAsync(int orderId, string userId);
        Task<List<OrderDto>> GetOrdersAsync(string userId);
        Task<OrderDto> TryUpdateStockAsync(PlaceOrderDto placeOrderDto);
    }
}
