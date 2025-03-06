﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services.Identity
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory) : AuthenticationStateProvider, IUserService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WebshopMinimalApi");
        private bool _authenticated = false;
        private readonly ClaimsPrincipal _unauthenticated = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
            var user = _unauthenticated;

            var userResponse = await _httpClient.GetAsync("Account/manage/info");
            try
            {
                userResponse.EnsureSuccessStatusCode();

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<AuthenticatedUserDto>(userJson, _jsonSerializerOptions);

                if (userInfo == null) return new AuthenticationState(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Email),
                    new Claim(ClaimTypes.Email, userInfo.Email)
                };
                claims.AddRange(
                    userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                        .Select(c => new Claim(c.Key, c.Value)));

                var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
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

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> UpdateUserInfo(UserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync("/Account/users/update", userDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }
    }
}
