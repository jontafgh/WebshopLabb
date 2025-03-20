using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.JSInterop;
using WebshopFrontend.Contracts;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class WebshopApiEndpoints
    {
        public const string Logout = "/account/logout";
        public const string Login = "/account/login";
        public const string Register = "/account/register";
        public const string GetUserClaims = "/account/claims";
        public const string GetUserInfo = "/account/manage/info";
        public const string PutUser = "/account/manage/details";
        public const string GetUser = "/account/manage/details";

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

    public class WebshopApiService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime) : IApiService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private async Task<HttpRequestMessage> CreateRequestWithAuthorizationToken(string endpoint, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);
            var session = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "session");

            if (string.IsNullOrWhiteSpace(session)) return request;

            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(session);

            if (loginResponse is null) return request;

            request.Headers.Authorization = new AuthenticationHeaderValue(loginResponse.TokenType, loginResponse.AccessToken);
            return request;
        }

        private StringContent SetRequestBody<TInput>(TInput data)
        {
            return new StringContent(JsonSerializer.Serialize(data, _jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");
        }

        private async Task<Result<TOutput>> ProcessResponse<TOutput>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return new Result<TOutput>
            {
                IsSuccess = false,
                ErrorMessage = $"{response.RequestMessage?.RequestUri} {response.RequestMessage?.Method}\n{(int)response.StatusCode} {response.ReasonPhrase}"
            };
            
            var responseContent = await response.Content.ReadAsStringAsync();

            if(string.IsNullOrWhiteSpace(responseContent))
            {
                return new Result<TOutput> { IsSuccess = true, Data = default};
            }

            try
            {
                var data = JsonSerializer.Deserialize<TOutput>(responseContent, _jsonSerializerOptions);
                return new Result<TOutput> { IsSuccess = true, Data = data };
            }
            catch (JsonException ex)
            {
                return new Result<TOutput> { IsSuccess = false, Data = default, ErrorMessage = $"Could not read JSON: {responseContent} {response.RequestMessage?.Method} {response.RequestMessage?.RequestUri} {ex.Message}"};
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, Data = default, ErrorMessage = $"{ex.Message}" };
            }
        }

        public async Task<Result<TOutput>> GetAsync<TOutput>(string url)
        {
            var message = await CreateRequestWithAuthorizationToken(url, HttpMethod.Get);

            try
            {
                var response = await _httpClient.SendAsync(message);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Result<TOutput>> PostAsync<TOutput, TInput>(string url, TInput data)
        {
            var message = await CreateRequestWithAuthorizationToken(url, HttpMethod.Post);
            message.Content = SetRequestBody(data);

            try
            {
                var response = await _httpClient.SendAsync(message);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<Result<TOutput>> PutAsync<TOutput, TInput>(string url, TInput data)
        {
            var message = await CreateRequestWithAuthorizationToken(url, HttpMethod.Put);
            message.Content = SetRequestBody(data);

            try
            {
                var response = await _httpClient.SendAsync(message);
                return await ProcessResponse<TOutput>(response);
            }
            catch (Exception ex)
            {
                return new Result<TOutput> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
        public async Task<Result<TOutput>> DeleteAsync<TOutput>(string url)
        {
            var message = await CreateRequestWithAuthorizationToken(url, HttpMethod.Delete);
            try
            {
                var response = await _httpClient.SendAsync(message);
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
