using Microsoft.AspNetCore.Cors.Infrastructure;
using WebshopShared;
using WebshopShared.Validation;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> UpdateUserDetails(UserDetails userDetails);
        public Task<RegisterLoginResponseDto> LoginAsync(LoginDto user);
        public Task LogoutAsync();
        public Task<RegisterLoginResponseDto> RegisterAsync(RegisterUserDto user);
        public Task<UserDetailsDto> GetUserDetails();
        public Task<bool> CheckAuthenticatedAsync();
    }
}
