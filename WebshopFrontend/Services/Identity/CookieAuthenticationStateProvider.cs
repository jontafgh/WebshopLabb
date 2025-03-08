using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;
using WebshopShared.Validation;

namespace WebshopFrontend.Services.Identity
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory, ICartService cartService) : AuthenticationStateProvider, IUserService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        private bool _authenticated = false;
        private readonly ClaimsPrincipal _unauthenticated = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
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
                    new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                    new Claim("CartId", JsonSerializer.Serialize(userInfo.CartId))
                };

                var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);

                if (!_authenticated)
                    await cartService.Login();

                _authenticated = true;
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
            var responseContent = await response.Content.ReadAsStringAsync();

            await cartService.Logout();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> UpdateUserDetails(UserDetails userDetails)
        {
            var response = await _httpClient.PutAsJsonAsync("/Account/users/update", userDetails);
            return response.IsSuccessStatusCode;
        }

        public async Task<UserDetailsDto> GetUserDetails()
        {
            var response = await _httpClient.GetAsync("/Account/users/details");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDetailsDto>(responseContent, _jsonSerializerOptions) ?? new UserDetailsDto();
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }
    }
}
