using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class UserService(IDbContextFactory<WebshopContext> dbContextFactory, UserManager<WebshopUser> userManager) : IUserService
    {
        public string? GetUserId(ClaimsPrincipal claims)
        {
            return claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<WebshopUser?> GetUserByIdAsync(string userId)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            return await dbContext.Users.FindAsync(userId);
        }

        public async Task<UserDetailsDto> UpdateUserAsync(UserDetailsDto userDetails, WebshopUser user)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.PhoneNumber = userDetails.PhoneNumber;
            user.Address = userDetails.Address?.ToAddress();

            await dbContext.SaveChangesAsync();

            userDetails.Email = user.Email;
            return userDetails;
        }

        public async Task<UserDetailsDto?> GetUserDetailsAsync(string userId)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            return await dbContext.Users.Where(u => u.Id == userId)
                .Include(u => u.Address)
                .Select(u => u.ToUserDetailsDto())
                .FirstOrDefaultAsync();
        }

        public async Task<AuthenticatedUserDto?> GetAuthenticatedUser(ClaimsPrincipal claims)
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
