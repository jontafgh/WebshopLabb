using System.Security.Claims;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface IUserService
    {
        string? GetUserId(ClaimsPrincipal claims);
        Task<WebshopUser?> GetUserByIdAsync(string userId);
        Task<UserDetailsDto?> UpdateUserAsync(string userId, UserDetailsDto userDetails);
        Task<UserDetailsDto?> GetUserDetailsAsync(string userId);
        Task<UserClaimsDto?> GetUserClaims(ClaimsPrincipal claimsPrincipal);
    }
}
