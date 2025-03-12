using WebshopBackend.Contracts;

namespace WebshopBackend.Endpoints
{
    public class ExchangeRateEndpoints(IExchangeRateService exchangeService) : IEndpoints
    {
        private const string BaseCode = "USD";

        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/exchangerates", async () =>
            {
                try
                {
                    var exchangeRates = await exchangeService.GetExchangeRatesAsync(BaseCode);
                    return Results.Ok(exchangeRates);
                }
                catch (Exception e)
                {
                    return Results.Problem($"Failed to get exchange rates, {e.Message}", statusCode: 500);
                }
            });

            app.MapGet("/exchangerates/{targetCode}", async (string targetCode) =>
            {
                try
                {
                    var exchangeRateDetails = await exchangeService.GetExchangeRateDetailsAsync(BaseCode, targetCode);
                    return Results.Ok(exchangeRateDetails);
                }
                catch (Exception e)
                {
                    return Results.Problem($"Failed to get exchange rate details, {e.Message}", statusCode: 500);
                }
            });
        }
    }
}
