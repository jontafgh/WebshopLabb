﻿using WebshopFrontend.Services.Interfaces;
using WebshopShared;

namespace WebshopFrontend.Services
{
    public class ProductService(HttpClient httpClient) : IProductService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ProductDto>> GetProducts()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>("products") ?? [];
        }
        public async Task<List<BoardgameDto>> GetBoardgames()
        {
            return await _httpClient.GetFromJsonAsync<List<BoardgameDto>>("boardgames") ?? [];
        }
        public async Task<BoardgameDto> GetBoardgameById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BoardgameDto>($"boardgames/{id}") ?? new BoardgameDto();
        }
        public async Task<BoardgameDto> GetBoardgameByArtNr(string artNr)
        {
            return await _httpClient.GetFromJsonAsync<BoardgameDto>($"boardgames/article/{artNr}") ?? new BoardgameDto();
        }
    }
}
