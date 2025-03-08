using Microsoft.EntityFrameworkCore;
using WebshopBackend.Contracts;
using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Services
{
    public class ProductService(WebshopContext dbContext) : IProductService
    {
        public async Task<List<Product>> GetProductsAsync()
        {
            return await dbContext.Products.Include(p => p.Image)
                .Include(p => p.Price)
                    .ThenInclude(p => p.Discount)
                .Include(p => p.Stock)
                    .ThenInclude(s => s.NextRestock)
                .ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await dbContext.Products.Include(p => p.Image)
                .Include(p => p.Price)
                    .ThenInclude(p => p.Discount)
                .Include(p => p.Stock)
                    .ThenInclude(s => s.NextRestock)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Boardgame>> GetBoardgamesAsync()
        {
            return await dbContext.Boardgames.Include(b => b.Publisher)
                .Include(b => b.Product)
                    .ThenInclude(b => b.Image)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Price)
                        .ThenInclude(p => p.Discount)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                .ToListAsync();
        }
        public async Task<Boardgame?> GetBoardgameByIdAsync(int id)
        {
            return await dbContext.Boardgames.Include(b => b.Publisher)
                .Include(b => b.Product)
                    .ThenInclude(b => b.Image)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Price)
                        .ThenInclude(p => p.Discount)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Boardgame?> GetBoardgameByArtNrAsync(string artNr)
        {
            return await dbContext.Boardgames.Include(b => b.Publisher)
                .Include(b => b.Product)
                    .ThenInclude(b => b.Image)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Price)
                        .ThenInclude(p => p.Discount)
                .Include(b => b.Product)
                    .ThenInclude(p => p.Stock)
                        .ThenInclude(s => s.NextRestock)
                .FirstOrDefaultAsync(b => b.Product!.ArtNr == artNr);
        }

        public async Task<Boardgame> AddBoardgameAsync(BoardgameDto boardgameDto)
        {
            var boardgame = boardgameDto.ToBoardgame();
            dbContext.Boardgames.Add(boardgame);
            await dbContext.SaveChangesAsync();
            return boardgame;
        }
    }
}
