using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class CookieAuthenticationStateProvider(IApiSevice webshopApi) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _unauthenticated = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _unauthenticated;

            var response = await webshopApi.GetAsync<AuthenticatedUserDto>(WebshopApiEndpoints.Authenticate);

            if (!response.IsSuccess || response.Data == null)
            {
                return new AuthenticationState(user);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, response.Data.Email),
                new Claim(ClaimTypes.Email, response.Data.Email),
                new Claim(ClaimTypes.NameIdentifier, response.Data.UserId)
            };

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
            user = new ClaimsPrincipal(id);
            return new AuthenticationState(user);
        }

        public async Task<RegisterLoginResponseDto> RegisterAsync(RegisterUserDto user)
        {
            var response = await webshopApi.PostAsync<string, RegisterUserDto>(WebshopApiEndpoints.Register, user);

            if (response.IsSuccess) { return new RegisterLoginResponseDto { Succeeded = true }; }
            
            var problemDetails = JsonDocument.Parse(response.Data!);

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
            var result = await webshopApi.PostAsync<string, LoginDto>(WebshopApiEndpoints.Login, user);

            if (result.IsSuccess)
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

            var response = await webshopApi.PostAsync<string, StringContent>(WebshopApiEndpoints.Logout, emptyContent);
            
            if (response.IsSuccess) NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
