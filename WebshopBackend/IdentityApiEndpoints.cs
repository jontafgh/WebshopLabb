using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        }
    }
}
