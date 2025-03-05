using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebshopBackend.Models;
using WebshopShared;
using static System.Net.WebRequestMethods;
namespace WebshopBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<WebshopUser>()
                .AddEntityFrameworkStores<WebshopContext>()
                .AddApiEndpoints();
                

            builder.Services.AddDbContext<WebshopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebshopDb"))
                );

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            
            var app = builder.Build();

            app.MapMinimalApiEndpoints();

            app.MapGroup("/Account").MapIdentityApi<WebshopUser>();

            app.MapGet("/Account/users/me", async (ClaimsPrincipal claims, WebshopContext context) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return await context.Users.FindAsync(userId);
            }).RequireAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
