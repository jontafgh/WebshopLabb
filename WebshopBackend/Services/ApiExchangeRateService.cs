using System.Text.Json;
using System.Text.Json.Nodes;
using WebshopBackend.Contracts;
using WebshopShared;
#pragma warning disable CA2208

namespace WebshopBackend.Services
{
    public class ApiExchangeRateService(IHttpClientFactory httpClientFactory) : IExchangeRateService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExchangeRateApi");
        public async Task<ExchangeRatesDto> GetExchangeRatesAsync(string baseCode)
        {
            var response = await _httpClient.GetAsync($"latest/{baseCode}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get exchange rates");
            }

            var json = await response.Content.ReadAsStreamAsync();

            try
            {
                var exchangeRateDto = await JsonSerializer.DeserializeAsync<ExchangeRatesDto>(json);
                return exchangeRateDto ?? throw new ArgumentNullException(nameof(json), "Could not parse json");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to parse exchange rates", e);
            }
        }

        public async Task<ExchangeRateDetailsDto> GetExchangeRateDetailsAsync(string baseCode, string targetCode)
        {
            var response = await _httpClient.GetAsync($"enriched/{baseCode}/{targetCode}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get exchange rate details");
            }

            var json = await response.Content.ReadAsStreamAsync();

            try
            {
                var jsonNode = await JsonNode.ParseAsync(json) ?? throw new ArgumentNullException(nameof(json), "Could not parse json");

                var targetData = jsonNode["target_data"]!.AsObject();
                
                return new ExchangeRateDetailsDto
                {
                    Country = targetData["locale"]!.GetValue<string>(),
                    CountryCode = targetData["two_letter_code"]!.GetValue<string>(),
                    CurrencyName = targetData["currency_name_short"]!.GetValue<string>(),
                    CurrencyUniCode = targetData["display_symbol"]!.GetValue<string>(),
                    FlagUrl = targetData["flag_url"]!.GetValue<string>()
                };
            }
            catch (Exception e)
            {
                throw new Exception("Failed to parse exchange rate details", e);
            }
        }
    }
}
