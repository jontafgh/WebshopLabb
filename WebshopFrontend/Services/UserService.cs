using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class UserService(HttpClient httpClient) : IUserService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> RegisterUser(RegisterUserDto user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/Account/register", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? true : throw new Exception($"Failed to register user: {responseContent}"); ;
        }

        public async Task<bool> LogInUser(LoginDto user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/Account/login?useCookies=true", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? true : throw new Exception($"Failed to login user: {responseContent}");
        }

        public async Task<bool> LogOutUser()
        {
            var response = await _httpClient.PostAsync("/Account/logout", null);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? true : throw new Exception($"Failed to logout user: {responseContent}");
        }

        public async Task<bool> GetIfLoggedIn()
        {
            var response = await _httpClient.GetAsync("/Account/users/me");
            return response.IsSuccessStatusCode;
        }
    }
}
