using System.ComponentModel.DataAnnotations.Schema;

namespace WebshopBackend.Models;

public class Product
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string ArtNr { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;
    public Image? Image { get; set; }
    public Price? Price { get; set; }
    public Stock? Stock { get; set; }
}

public class Image
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    [Column(TypeName = "nvarchar(500)")]
    public string ImageText { get; set; } = null!;

    [Column(TypeName = "nvarchar(500)")]
    public string ImageSmallUrl { get; set; } = null!;

    [Column(TypeName = "nvarchar(500)")]
    public string ImageMediumUrl { get; set; } = null!;

    [Column(TypeName = "nvarchar(500)")]
    public string ImageLargeUrl { get; set; } = null!;
}

public class Price
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Regular { get; set; }
    public Discount? Discount { get; set; }
}

public class Discount
{
    public int Id { get; set; }
    public int? PriceId { get; set; }
    public Price? Price { get; set; }

    [Column(TypeName = "money")]
    public decimal DiscountPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class Stock
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public Restock? NextRestock { get; set; }
}

public class Restock
{
    public int Id { get; set; }
    public int? StockId { get; set; }
    public Stock? Stock { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime RestockDate { get; set; }
}