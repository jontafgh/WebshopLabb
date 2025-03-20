using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebshopBackend.Contracts;
using WebshopShared;
using WebshopBackend.Models;

namespace WebshopBackend.Endpoints
{
    public class UserEndpoints(IUserService userService) : IEndpoints
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapPost("Account/logout", async (SignInManager<WebshopUser> signInManager, [FromBody] object? empty) =>
            {
                if (empty == null) return Results.Unauthorized();

                await signInManager.SignOutAsync();

                return Results.Ok();

            }).RequireAuthorization();

            app.MapPut("/Account/users/update", async (ClaimsPrincipal claims, UserDetailsDto user) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId == null) return Results.Unauthorized();

                var userDetails = await userService.UpdateUserAsync(userId, user);
                return Results.Ok(userDetails);

            }).RequireAuthorization();

            app.MapGet("/Account/users/details", async (ClaimsPrincipal claims) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId == null) return Results.Unauthorized();
               
                var user = await userService.GetUserDetailsAsync(userId);
                return user == null ? Results.NotFound() : Results.Ok(user);

            }).RequireAuthorization();
            
            app.MapGet("/Account/users/claims", async (ClaimsPrincipal user) =>
            {
                var claims = await userService.GetUserClaims(user);
                return claims == null ? Results.Unauthorized() : Results.Ok(claims);

            }).RequireAuthorization();
        }
    }
}
