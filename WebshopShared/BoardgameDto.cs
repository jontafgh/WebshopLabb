using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class BoardgameDto 
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAge { get; set; }
        public int PlayTime { get; set; }
        public PublisherDto Publisher { get; set; } = null!;
        public ProductDto Product { get; set; } = null!;
    }

    public class ProductDto 
    {
        public int Id { get; set; }
        public string ArtNr { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ImageDto Image { get; set; } = null!;
        public PriceDto Price { get; set; } = null!;
        public StockDto Stock { get; set; } = null!;
    }

    public class PublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ImageDto
    {
        public int Id { get; set; }
        public string ImageText { get; set; } = null!;
        public string ImageSmallUrl { get; set; } = null!;
        public string ImageMediumUrl { get; set; } = null!;
        public string ImageLargeUrl { get; set; } = null!;
    }

    public class PriceDto
    {
        public int Id { get; set; }
        public decimal Regular { get; set; }
        public DiscountDto? Discount { get; set; } = null!;
    }

    public class DiscountDto
    {
        public int Id { get; set; }
        public decimal DiscountPrice { get; set; } 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }

    public class StockDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public RestockDto? NextRestock { get; set; } = null!;
    }

    public class RestockDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime RestockDate { get; set; }
    }
}
