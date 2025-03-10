namespace WebshopBackend.Models;

public class Address
{
    public int Id { get; set; }
    public List<WebshopUser> Users { get; set; } = [];
    public string Street { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}