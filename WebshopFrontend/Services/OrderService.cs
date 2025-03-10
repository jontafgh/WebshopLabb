using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class OrderService(ICartService cartService, IHttpClientFactory httpClientFactory) : IOrderService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        public Task<List<OrderDto>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDto> GetOrder(int orderId)
        {
            var response = await _httpClient.GetAsync($"/order/{orderId}");

            if (!response.IsSuccessStatusCode) return new OrderDto();

            return await response.Content.ReadFromJsonAsync<OrderDto>() ?? new OrderDto();
        }

        public async Task<OrderDto> PlaceOrder()
        {
            var order = await CreatePlaceOrderDto();
            var response = await _httpClient.PostAsJsonAsync("/order", order);

            if (!response.IsSuccessStatusCode) return new OrderDto();

            return await response.Content.ReadFromJsonAsync<OrderDto>() ?? new OrderDto();
        }

        private async Task<PlaceOrderDto> CreatePlaceOrderDto()
        {
            var cartItems = await cartService.GetCart();

            return new PlaceOrderDto { CartItems = cartItems };
        }
    }
}
