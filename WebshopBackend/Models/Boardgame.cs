using WebshopShared;

namespace WebshopBackend.Models
{
    public class Boardgame
    {
        public int Id { get; set; }
        public string ArtNr { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Publisher? Publisher { get; set; } 
        public Image? Image { get; set; } 
        public Price? Price { get; set; } 
        public BoardgameDetails? BoardgameDetails { get; set; }
        public Stock? Stock { get; set; } 
    }

    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class Image
    {
        public int Id { get; set; }
        public string ImageText { get; set; } = null!;
        public string ImageSmallUrl { get; set; } = null!;
        public string ImageMediumUrl { get; set; } = null!;
        public string ImageLargeUrl { get; set; } = null!;
    }

    public class Price
    {
        public int Id { get; set; }
        public decimal Regular { get; set; }
        public Discount? Discount { get; set; }
    }

    public class Discount
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BoardgameDetails
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAge { get; set; }
        public int PlayTime { get; set; }
    }

    public class Stock
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Restock? NextRestock { get; set; }
    }

    public class Restock
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime RestockDate { get; set; }
    }
}
