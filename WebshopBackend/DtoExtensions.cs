using WebshopBackend.Models;
using WebshopShared;

namespace WebshopBackend;

public static class DtoExtensions
{
    public static ProductDto ToProductDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            ArtNr = product.ArtNr,
            Name = product.Name,
            Image = new ImageDto
            {
                ImageText = product.Image.ImageText,
                ImageSmallUrl = product.Image.ImageSmallUrl,
                ImageMediumUrl = product.Image.ImageMediumUrl,
                ImageLargeUrl = product.Image.ImageLargeUrl
            },
            Price = new PriceDto
            {
                Regular = product.Price.Regular,
                Discount = (product.Price.Discount == null) ? null : new DiscountDto
                {
                    DiscountPrice = product.Price.Discount.DiscountPrice,
                    StartDate = product.Price.Discount.StartDate,
                    EndDate = product.Price.Discount.EndDate
                }
            },
            Stock = new StockDto
            {
                Quantity = product.Stock.Quantity,
                NextRestock = (product.Stock.NextRestock == null) ? null : new RestockDto
                {
                    Quantity = product.Stock.NextRestock.Quantity,
                    RestockDate = product.Stock.NextRestock.RestockDate
                }
            }
        };
    }
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