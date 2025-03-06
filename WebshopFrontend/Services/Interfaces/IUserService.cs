using Microsoft.AspNetCore.Cors.Infrastructure;
using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IUserService
    {
        //public Task<bool> RegisterUser(RegisterUserDto user);
        //public Task<bool> LogInUser(LoginDto user);
        //public Task<bool> LogOutUser();
        //public Task<bool> GetIfLoggedIn();
        //public Task<string> GetUserId();
        public Task<bool> UpdateUserInfo(UserDto userDto);
        public Task<RegisterLoginResponseDto> LoginAsync(LoginDto user);
        public Task LogoutAsync();
        public Task<RegisterLoginResponseDto> RegisterAsync(RegisterUserDto user);
        public Task<bool> CheckAuthenticatedAsync();
    }
}
