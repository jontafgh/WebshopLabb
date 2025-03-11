using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using WebshopFrontend.Contracts;
using WebshopFrontend.Razor;
using WebshopFrontend.Services;

namespace WebshopFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddAuthenticationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
        //builder.Services.AddScoped(
        //    sp => (IUserService)sp.GetRequiredService<AuthenticationStateProvider>());

        builder.Services.AddHttpClient("WebshopMinimalApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5057/");
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        });

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddSingleton<ICounterService, CartCounterService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddKeyedScoped<ICartService, ApiCartService>("Api");
        builder.Services.AddKeyedScoped<ICartService, CartService>("Local");

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
