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
            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user = await context.Users.FindAsync(userId);
                return user.ToUserDto();
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

            app.MapGet("/Account/users/{id}", async (string id, WebshopContext context) =>
            {
                var user = await context.Users.FindAsync(id);
                if (user == null) return Results.NotFound();
                var userDto = user.ToUserDto();
                return Results.Ok(userDto);
            }).RequireAuthorization();
        }
    }
}
