using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend.Contracts
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Boardgame>> GetBoardgamesAsync();
        Task<Boardgame?> GetBoardgameByIdAsync(int id);
        Task<Boardgame?> GetBoardgameByArtNrAsync(string artNr);
        Task<Boardgame> AddBoardgameAsync(BoardgameDto boardgame);
    }
}
