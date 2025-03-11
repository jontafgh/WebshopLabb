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

                var userToUpdate = await userService.GetUserByIdAsync(userId);
                if (userToUpdate == null) return Results.NotFound();

                var userDetails = await userService.UpdateUserAsync(user, userToUpdate);
                return Results.Ok(userDetails);

            }).RequireAuthorization();

            app.MapGet("/Account/users/details", async (ClaimsPrincipal claims) =>
            {
                var userId = userService.GetUserId(claims);
                if (userId == null) return Results.Unauthorized();
               
                var user = await userService.GetUserDetailsAsync(userId);
                return user == null ? Results.NotFound() : Results.Ok(user);

            }).RequireAuthorization();

            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims) =>
            {
                var authenticatedUserDto = await userService.GetUserClaimsAsync(claims);
                return authenticatedUserDto == null ? Results.Unauthorized() : Results.Ok(authenticatedUserDto);

            }).RequireAuthorization();
        }
    }
}
