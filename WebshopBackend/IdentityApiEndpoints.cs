using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopShared;

namespace WebshopBackend
{
    public static class IdentityApiEndpoints
    {
        public static void MapMyIdentityApiEndpoints(this WebApplication app)
        {
            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return await context.Users.FindAsync(userId);
            }).RequireAuthorization();

            app.MapPost("Account/logout", async (SignInManager<WebshopUser> signInManager, [FromBody] object? empty) =>
            {
                if (empty == null) return Results.Unauthorized();
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization();

            app.MapPost("Account/userclaims", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var user = await context.Users.FindAsync(claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier));
                if (user == null) return Results.Unauthorized();
                var userClaims = new UserClaimsDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!
                };
                return Results.Content(Convert.ToString(userClaims));
            });
        }
    }
}
