using WebshopFrontend.Components;
using WebshopFrontend.EventHandlers;
using WebshopFrontend.Services;
using WebshopFrontend.Services.Interfaces;

namespace WebshopFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddScoped<NotifyCartUpdated>();
        builder.Services.AddScoped<OrderState>();
        builder.Services.AddScoped<ICartService, LocalStorageCartService>();
        builder.Services.AddSingleton<ICounterService, CartCounterService>();

        builder.Services.AddHttpClient("WebshopMinimalApi", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7003/");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
