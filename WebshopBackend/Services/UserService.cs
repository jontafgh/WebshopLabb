using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class UserService(WebshopContext dbContext, UserManager<WebshopUser> userManager) : IUserService
    {
        public string? GetUserId(ClaimsPrincipal claims)
        {
            return claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<WebshopUser?> GetUserByIdAsync(string userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(UserDetailsDto userDetails, WebshopUser user)
        {
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.PhoneNumber = userDetails.PhoneNumber;
            user.Address = userDetails.Address?.ToAddress();
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserDetailsDto?> GetUserDetailsAsync(string userId)
        {
            return await dbContext.Users.Where(u => u.Id == userId)
                .AsNoTracking()
                .Include(u => u.Address)
                .Select(u => u.ToUserDetailsDto())
                .FirstOrDefaultAsync();
        }

        public async Task<AuthenticatedUserDto?> GetUserClaimsAsync(ClaimsPrincipal claims)
        {
            var validUser = await userManager.GetUserAsync(claims);
            if (validUser == null) return null;

            return new AuthenticatedUserDto
            {
                UserId = validUser.Id,
                Email = validUser.Email!
            };
        }
    }
}
