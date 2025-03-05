using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IBoardgameService
    {
        public Task<BoardgameDto?> GetBoardgameById(int id);
        public Task<BoardgameDto?> GetBoardgameByArtNr(string artNr);
        public Task<List<BoardgameDto?>> GetBoardgames();
    }
}
