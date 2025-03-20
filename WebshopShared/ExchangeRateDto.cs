using System.Text.Json.Serialization;

namespace WebshopShared
{
    public class ExchangeRatesDto
    {
        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; } = null!;
        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> Rates { get; set; } = [];
    }

    public class ExchangeRateDetailsDto
    {
        public string Country { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string CurrencyUniCode { get; set; } = null!;
        public string FlagUrl { get; set; } = null!;     
    }
}
