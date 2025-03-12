using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface IExchangeRateService
    {
        Task SetExchangeRatesAsync();
        void SetCurrentRate(KeyValuePair<string, decimal> selectedRate);
        KeyValuePair<string, decimal> GetCurrentRate();
        Dictionary<string, decimal> GetExchangeRates();

        public event EventHandler<KeyValuePair<string, decimal>> ExchangeRateChanged;
    }
}
