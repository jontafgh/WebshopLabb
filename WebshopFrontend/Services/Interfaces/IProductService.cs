using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProducts();
        public Task<BoardgameDto> GetBoardgameById(int id);
        public Task<BoardgameDto> GetBoardgameByArtNr(string artNr);
        public Task<List<BoardgameDto>> GetBoardgames();
    }
}
