using System.Text.Json;
using WebshopFrontend.Contracts;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class ExchangeRateService(IHttpClientFactory httpClientFactory) : IExchangeRateService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");

        public event EventHandler<KeyValuePair<string, decimal>>? ExchangeRateChanged;

        public Dictionary<string, decimal> Rates = new Dictionary<string, decimal>();
        public KeyValuePair<string, decimal> CurrentRate;

        public async Task SetExchangeRatesAsync()
        {
            var response = await _httpClient.GetAsync(WebshopApiEndpoints.GetExchangerates);
            if (!response.IsSuccessStatusCode)
            {
                CurrentRate = new KeyValuePair<string, decimal>("USD", 1);
                Rates.Add("USD", 1);
            }

            var exchangeRates = await JsonSerializer.DeserializeAsync<ExchangeRatesDto>(await response.Content.ReadAsStreamAsync());

            Rates = exchangeRates?.Rates ?? new Dictionary<string, decimal>();

            CurrentRate = new KeyValuePair<string, decimal>(exchangeRates!.BaseCode, 1);
        }

        public void SetCurrentRate(KeyValuePair<string, decimal> selectedRate)
        {
            CurrentRate = selectedRate;
            ExchangeRateChanged?.Invoke(this, CurrentRate);
        }

        public KeyValuePair<string, decimal> GetCurrentRate()
        {
            return CurrentRate;
        }

        public Dictionary<string, decimal> GetExchangeRates()
        {
            return Rates;
        }
    }
}
