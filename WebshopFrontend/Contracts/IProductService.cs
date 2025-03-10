using WebshopShared;

namespace WebshopFrontend.Contracts
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProducts();
        public Task<ProductDto> GetProductById(int id);
        public Task<BoardgameDto> GetBoardgameById(int id);
        public Task<BoardgameDto> GetBoardgameByArtNr(string artNr);
        public Task<List<BoardgameDto>> GetBoardgames();
    }
}
