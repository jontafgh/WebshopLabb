using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebshopShared;

namespace WebshopBackend
{
    public static class IdentityApiEndpoints
    {
        public static void MapMyIdentityApiEndpoints(this WebApplication app)
        {
            app.MapPost("Account/logout", async (SignInManager<WebshopUser> signInManager, [FromBody] object? empty) =>
            {
                if (empty == null) return Results.Unauthorized();
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization();

            app.MapPut("/Account/users/update", async (UserDto user, WebshopContext context) =>
            {
                var dbUser = await context.Users.FindAsync(user.Id);
                if (dbUser == null) return Results.NotFound();
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.PhoneNumber = user.PhoneNumber;
                dbUser.Address = user.Address.ToAddress();
                await context.SaveChangesAsync();
                return Results.Ok();

            }).RequireAuthorization();

            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = await context.Users.Where(u => u.Id == userId)
                    .Include(u => u.Cart)
                    .FirstOrDefaultAsync();
                if (user == null) return Results.NotFound();

                var authenticatedUserDto = new AuthenticatedUserDto
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    CartId = user.Cart?.Id ?? 0,
                };
                return Results.Ok(authenticatedUserDto);

            }).RequireAuthorization();
        }
    }
}
