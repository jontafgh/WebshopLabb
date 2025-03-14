using System.Text.Json;
using WebshopFrontend.Contracts;
using WebshopShared.Validation;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class UserService(IHttpClientFactory httpClientFactory) : IUserService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<UserDetailsDto> UpdateUserDetails(UserDetails userDetails)
        {
            var response = await _httpClient.PutAsJsonAsync("/Account/users/update", userDetails);
            if (!response.IsSuccessStatusCode) return new UserDetailsDto();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDetailsDto>(responseContent, _jsonSerializerOptions) ?? new UserDetailsDto();
        }

        public async Task<UserDetailsDto> GetUserDetails()
        {
            return await _httpClient.GetFromJsonAsync<UserDetailsDto>("/Account/users/details", _jsonSerializerOptions) ?? new UserDetailsDto();
        }
    }
}
