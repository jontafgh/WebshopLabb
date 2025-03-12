using System.Security.Claims;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface IUserService
    {
        string? GetUserId(ClaimsPrincipal claims);
        Task<WebshopUser?> GetUserByIdAsync(string userId);
        Task<UserDetailsDto> UpdateUserAsync(UserDetailsDto userDetails, WebshopUser user);
        Task<UserDetailsDto?> GetUserDetailsAsync(string userId);
        Task<AuthenticatedUserDto?> GetAuthenticatedUser(ClaimsPrincipal claims);
    }
}
