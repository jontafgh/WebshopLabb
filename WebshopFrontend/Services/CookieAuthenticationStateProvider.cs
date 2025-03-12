using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory) : AuthenticationStateProvider
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        private readonly ClaimsPrincipal _unauthenticated = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _unauthenticated;

            var userResponse = await _httpClient.GetAsync("/Account/users/me");
            
            try
            {
                userResponse.EnsureSuccessStatusCode();

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<AuthenticatedUserDto>(userJson, _jsonSerializerOptions);

                if (userInfo == null) return new AuthenticationState(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Email),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.UserId)
                };

                var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new AuthenticationState(user);
        }

        public async Task<RegisterLoginResponseDto> RegisterAsync(RegisterUserDto user)
        {
            var result = await _httpClient.PostAsJsonAsync("Account/register", user);
            if (result.IsSuccessStatusCode) { return new RegisterLoginResponseDto { Succeeded = true }; }

            var details = await result.Content.ReadAsStringAsync();
            var problemDetails = JsonDocument.Parse(details);

            var errors = new List<string>();
            var errorList = problemDetails.RootElement.GetProperty("errors");
            errorList.EnumerateObject().ToList().ForEach(errorEntry =>
            {
                if (errorEntry.Value.ValueKind == JsonValueKind.String)
                {
                    errors.Add(errorEntry.Value.GetString()!);
                }
                else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                {
                    errors.AddRange(errorEntry.Value.EnumerateArray().Select(e => e.GetString() ?? string.Empty).Where(e => !string.IsNullOrEmpty(e)));
                }
            });
            
            return new RegisterLoginResponseDto
            {
                Succeeded = false,
                ErrorList = errors
            };
        }

        public async Task<RegisterLoginResponseDto> LoginAsync(LoginDto user)
        {
            var result = await _httpClient.PostAsJsonAsync("/Account/login?useCookies=true", user);

            if (result.IsSuccessStatusCode)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return new RegisterLoginResponseDto { Succeeded = true };
            }
            return new RegisterLoginResponseDto
            {
                Succeeded = false,
                ErrorList = ["Invalid email and/or password."]
            };
        }

        public async Task LogoutAsync()
        {
            const string empty = "{}";
            var emptyContent = new StringContent(empty, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/Account/logout", emptyContent);
            await response.Content.ReadAsStringAsync();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
