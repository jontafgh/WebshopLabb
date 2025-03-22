using System.ComponentModel.DataAnnotations.Schema;

namespace WebshopBackend.Models;

public class Address
{
    public int Id { get; set; }
    public List<WebshopUser> Users { get; set; } = [];

    [Column(TypeName = "nvarchar(200)")]
    public string Street { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string PostalCode { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string City { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string Country { get; set; } = null!;
}