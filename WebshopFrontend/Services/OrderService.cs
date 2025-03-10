using WebshopFrontend.Contracts;
using WebshopFrontend.Razor.Pages.Checkout;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class OrderService(IHttpClientFactory httpClientFactory) : IOrderService
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

        public async Task<OrderDto> PlaceOrder(List<CartItemDto> cartItems)
        {
            var order = new PlaceOrderDto { CartItems = cartItems };
            var response = await _httpClient.PostAsJsonAsync("/order", order);

            if (!response.IsSuccessStatusCode) return new OrderDto();

            return await response.Content.ReadFromJsonAsync<OrderDto>() ?? new OrderDto();
        }
    }
}
