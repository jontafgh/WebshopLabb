using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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

            var connectionStringPc = builder.Configuration.GetConnectionString("WebshopDbPc");
            var connectionStringLaptop = builder.Configuration.GetConnectionString("WebshopDbLaptop");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<WebshopUser>()
                .AddEntityFrameworkStores<WebshopContext>()
                .AddApiEndpoints();
                

            builder.Services.AddDbContext<WebshopContext>(options =>
                options.UseSqlServer(connectionStringPc)
                );

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            
            var app = builder.Build();

            app.MapMinimalApiEndpoints();

            app.MapGroup("/Account").MapIdentityApi<WebshopUser>();
            app.MapMyIdentityApiEndpoints();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
