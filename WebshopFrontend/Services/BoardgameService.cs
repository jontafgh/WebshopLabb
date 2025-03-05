using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class BoardgameService(HttpClient httpClient) : IBoardgameService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<BoardgameDto?>> GetBoardgames()
        {
            return await _httpClient.GetFromJsonAsync<List<BoardgameDto?>>("products") ?? [];
        }

        public async Task<BoardgameDto?> GetBoardgameById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BoardgameDto>($"products/{id}");
        }

        public async Task<BoardgameDto?> GetBoardgameByArtNr(string artNr)
        {
            return await _httpClient.GetFromJsonAsync<BoardgameDto>($"products/article/{artNr}");
        }
    }
}
