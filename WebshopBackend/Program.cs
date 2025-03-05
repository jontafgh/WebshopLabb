using System.Text.Json.Serialization;
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

            builder.Services.AddDbContext<WebshopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebshopDb"))
                );

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            var app = builder.Build();

            app.MapMinimalApiEndpoints();

            app.Run();
        }
    }
}
