using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Endpoints;
using WebshopBackend.Models;
using WebshopBackend.Services;

namespace WebshopBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionStringPc = builder.Configuration.GetConnectionString("WebshopDbPc");
            //var connectionStringLaptop = builder.Configuration.GetConnectionString("WebshopDbLaptop");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContextFactory<WebshopContext>(options =>

                options.UseSqlServer(connectionStringPc)
                    //.LogTo(message => Debug.WriteLine(message))
                    //.EnableSensitiveDataLogging()
            );

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<WebshopUser>()
                .AddEntityFrameworkStores<WebshopContext>()
                .AddApiEndpoints();
            
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddHttpClient("ExchangeRateApi",
                client => client.BaseAddress = new Uri("https://v6.exchangerate-api.com/v6/c8f271e29a347411d479c7e6/"));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSingleton<IExchangeRateService, ApiExchangeRateService>();

            builder.Services.AddScoped<IEndpoints, UserEndpoints>();
            builder.Services.AddScoped<IEndpoints, CartEndpoints>();
            builder.Services.AddScoped<IEndpoints, ProductEndpoints>();
            builder.Services.AddScoped<IEndpoints, OrderEndpoints>();
            builder.Services.AddScoped<IEndpoints, ExchangeRateEndpoints>();

            var app = builder.Build();

            app.MapGroup("/Account").MapIdentityApi<WebshopUser>();
            app.MapMyEndpoints();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
