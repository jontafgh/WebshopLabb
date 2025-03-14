using System.Text.Json;
using WebshopFrontend.Contracts;

namespace WebshopFrontend.Services
{
    public class WebshopApiEndpoints
    {
        public const string Logout = "/Account/logout";
        public const string Login = "/Account/login?useCookies=true";
        public const string Register = "/Account/register";
        public const string Authenticate = "/Account/users/me";

        public const string PutUser = "/Account/users/update";
        public const string GetUser = "/Account/users/details";

        public const string GetProducts = "/products";
        public const string GetProduct = "/products/{0}";
        public const string GetBoardgames = "/boardgames";
        public const string GetBoardgame = "/boardgames/{0}";
        public const string GetBoardgameByArtNr = "/boardgames/article/{0}";

        public const string GetOrders = "/orders";
        public const string GetOrder = "/order/{0}";
        public const string PostOrder = "/order";

        public const string GetCart = "/cart";
        public const string DeleteCart = "/cart";
        public const string PutCart = "/cart";

        public const string GetExchangerates = "/exchangerates";
    }

    public class WebshopApiService(IHttpClientFactory httpClientFactory) : IApiService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private async Task<Result<TOutput>> ProcessResponse<TOutput>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return new Result<TOutput> { IsSuccess = false, ErrorMessage = $"{response.StatusCode} {response.ReasonPhrase}" };
            
            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var data = JsonSerializer.Deserialize<TOutput>(responseContent, _jsonSerializerOptions);
                return new Result<TOutput> { IsSuccess = true, Data = data };
            }
            catch (JsonException ex)
            {
                return new Result<TOutput> { IsSuccess = true, Data = default, ErrorMessage = ex.Message};
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = true, Data = default, ErrorMessage = $"{response.StatusCode} {response.ReasonPhrase} {ex.Message}" };
            }
        }

        public async Task<Result<TOutput>> GetAsync<TOutput>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Result<TOutput>> PostAsync<TOutput, TInput>(string url, TInput data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, data);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Result<TOutput>> PutAsync<TOutput, TInput>(string url, TInput data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, data);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<Result<TOutput>> DeleteAsync<TOutput>(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }

    public class Result <TOutput>
    {
        public bool IsSuccess { get; set; }
        public TOutput? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
