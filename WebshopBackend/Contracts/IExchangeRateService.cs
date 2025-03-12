using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface IExchangeRateService
    {
        Task<ExchangeRatesDto> GetExchangeRatesAsync(string baseCode);
        Task<ExchangeRateDetailsDto> GetExchangeRateDetailsAsync(string baseCode, string targetCode);
    }
}
