using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopShared;

namespace WebshopBackend.Endpoints
{
    public class ProductEndpoints(IProductService productService) : IEndpoints
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/products", async () =>
            {
                var products = await productService.GetProductsAsync();
                return Results.Ok(products);
            });

            app.MapGet("/products/{id:int}", async (int id) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            app.MapGet("/boardgames", async () =>
            {
                var boardgames = await productService.GetBoardgamesAsync();
                return Results.Ok(boardgames);
            });

            app.MapGet("/boardgames/{id:int}", async (int id) =>
            {
                var boardgame = await productService.GetBoardgameByIdAsync(id);
                return boardgame is not null ? Results.Ok(boardgame) : Results.NotFound();
            });

            app.MapGet("/boardgames/article/{artNr}", async (string artNr) =>
            {
                var boardgame = await productService.GetBoardgameByArtNrAsync(artNr);
                return boardgame is not null ? Results.Ok(boardgame) : Results.NotFound();
            });

            app.MapPost("/boardgames", async (BoardgameDto boardgameDto) =>
            {
                var boardgame = await productService.AddBoardgameAsync(boardgameDto);
                return Results.Created($"/boardgames/{boardgame.Id}", boardgame);
            });
        }
    }
}
