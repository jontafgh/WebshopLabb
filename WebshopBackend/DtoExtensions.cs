using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend;

public static class DtoExtensions
{
    public static Boardgame ToBoardgame(this BoardgameDto boardgameDto)
    {
        return new Boardgame
        {
            Description = boardgameDto.BoardgameDetails.Description,
            MinPlayers = boardgameDto.BoardgameDetails.MinPlayers,
            MaxPlayers = boardgameDto.BoardgameDetails.MaxPlayers,
            MinAge = boardgameDto.BoardgameDetails.MinAge,
            PlayTime = boardgameDto.BoardgameDetails.PlayTime,
            Product = boardgameDto.ToProduct(),
            Publisher = new Publisher
            {
                Name = boardgameDto.Publisher.Name
            }
        };
    }
    public static Product ToProduct(this BoardgameDto boardgameDto)
    {
        return new Product
        {
            ArtNr = boardgameDto.ArtNr,
            Name = boardgameDto.Name,
            Image = new Image
            {
                ImageText = boardgameDto.Image.ImageText,
                ImageSmallUrl = boardgameDto.Image.ImageSmallUrl,
                ImageMediumUrl = boardgameDto.Image.ImageMediumUrl,
                ImageLargeUrl = boardgameDto.Image.ImageLargeUrl
            },
            Price = new Price
            {
                Regular = boardgameDto.Price.Regular,
                Discount = (boardgameDto.Price.Discount == null) ? null : new Discount
                {
                    DiscountPrice = boardgameDto.Price.Discount.Price,
                    StartDate = boardgameDto.Price.Discount.StartDate,
                    EndDate = boardgameDto.Price.Discount.EndDate
                }
            },
            Stock = new Stock
            {
                Quantity = boardgameDto.Stock.Quantity,
                NextRestock = (boardgameDto.Stock.NextRestock == null) ? null : new Restock
                {
                    Quantity = boardgameDto.Stock.NextRestock.Quantity,
                    RestockDate = boardgameDto.Stock.NextRestock.RestockDate
                }
            }
        };
    }
}