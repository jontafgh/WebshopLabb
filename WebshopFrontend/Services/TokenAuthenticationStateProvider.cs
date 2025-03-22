using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using WebshopFrontend.Contracts;
using WebshopShared;
using WebshopShared.Validation;

namespace WebshopFrontend.Services
{
    public class TokenAuthenticationStateProvider(IApiService webshopApi, IJSRuntime jsRuntime) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            var session = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "session");
            if (string.IsNullOrWhiteSpace(session)) return new AuthenticationState(new ClaimsPrincipal(identity));

            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(session);
            if (loginResponse?.Claims is null) return new AuthenticationState(new ClaimsPrincipal(identity));

            if (!await ValidateUser()) return new AuthenticationState(new ClaimsPrincipal(identity));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.Claims.UserName),
                new Claim(ClaimTypes.Email, loginResponse.Claims.Email),
                new Claim(ClaimTypes.NameIdentifier, loginResponse.Claims.Id)
            };

            identity = new ClaimsIdentity(claims, nameof(TokenAuthenticationStateProvider));
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        private async Task<bool> ValidateUser()
        {
            var result = await webshopApi.GetAsync<UserInfoDto>(WebshopApiEndpoints.GetUserInfo);
            return result.IsSuccess;
        }

        public async Task<RegisterLoginResponseDto> RegisterAsync(RegisterUserDto user)
        {
            var response = await webshopApi.PostAsync<string, RegisterUserDto>(WebshopApiEndpoints.Register, user);
            if (response.IsSuccess) { return new RegisterLoginResponseDto { Succeeded = true }; }
            
            var problemDetails = JsonDocument.Parse(response.StringData!);
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
            var result = await webshopApi.PostAsync<LoginResponseDto, LoginDto>(WebshopApiEndpoints.Login, user);

            if (result is { IsSuccess: true, Data: not null })
            {
                var loginResponseJson = JsonSerializer.Serialize(result.Data);
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "session", loginResponseJson);

                var claims = await webshopApi.GetAsync<UserClaimsDto>(WebshopApiEndpoints.GetUserClaims);

                if (claims.Data is not null) result.Data.Claims = claims.Data;

                loginResponseJson = JsonSerializer.Serialize(result.Data);
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "session", loginResponseJson);

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

            if (response.IsSuccess)
            {
                await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "session");
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }
    }
}
