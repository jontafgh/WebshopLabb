using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend;

public static class DtoExtensions
{
    public static Boardgame ToBoardgame(this BoardgameDto boardgameDto)
    {
        return new Boardgame
        {
            Description = boardgameDto.Description,
            MinPlayers = boardgameDto.MinPlayers,
            MaxPlayers = boardgameDto.MaxPlayers,
            MinAge = boardgameDto.MinAge,
            PlayTime = boardgameDto.PlayTime,
            Publisher = new Publisher
            {
                Name = boardgameDto.Publisher.Name
            },
            Product = boardgameDto.ToProduct(),
            
        };
    }
    public static Product ToProduct(this BoardgameDto boardgameDto)
    {
        return new Product
        {
            ArtNr = boardgameDto.Product.ArtNr,
            Name = boardgameDto.Product.Name,
            Image = new Image
            {
                ImageText = boardgameDto.Product.Image.ImageText,
                ImageSmallUrl = boardgameDto.Product.Image.ImageSmallUrl,
                ImageMediumUrl = boardgameDto.Product.Image.ImageMediumUrl,
                ImageLargeUrl = boardgameDto.Product.Image.ImageLargeUrl
            },
            Price = new Price
            {
                Regular = boardgameDto.Product.Price.Regular,
                Discount = (boardgameDto.Product.Price.Discount == null) ? null : new Discount
                {
                    DiscountPrice = boardgameDto.Product.Price.Discount.DiscountPrice,
                    StartDate = boardgameDto.Product.Price.Discount.StartDate,
                    EndDate = boardgameDto.Product.Price.Discount.EndDate
                }
            },
            Stock = new Stock
            {
                Quantity = boardgameDto.Product.Stock.Quantity,
                NextRestock = (boardgameDto.Product.Stock.NextRestock == null) ? null : new Restock
                {
                    Quantity = boardgameDto.Product.Stock.NextRestock.Quantity,
                    RestockDate = boardgameDto.Product.Stock.NextRestock.RestockDate
                }
            }
        };
    }
}