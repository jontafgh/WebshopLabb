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

            app.MapPut("/Account/users/update", async (ClaimsPrincipal claims, UserDetailsDto user, WebshopContext context) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var userToUpdate = await context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

                if (userToUpdate == null)
                {
                    return Results.NotFound();
                }

                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.PhoneNumber = user.PhoneNumber;
                userToUpdate.Address = user.Address.ToAddress();

                await context.SaveChangesAsync();
                return Results.Ok();

            }).RequireAuthorization();

            app.MapGet("/Account/users/details", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var user = await context.Users.Where(u => u.Id == userId)
                    .Include(u => u.Address)
                    .Select(u => u.ToUserDetailsDto())
                    .FirstOrDefaultAsync();

                return user == null ? Results.NotFound() : Results.Ok(user);

            }).RequireAuthorization();

            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var user = await context.Users.Where(u => u.Id == userId)
                    .Include(u => u.Cart)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return Results.NotFound();
                }

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
