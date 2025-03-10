using Microsoft.AspNetCore.Identity;

namespace WebshopBackend.Models;

public class WebshopUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    public Cart? Cart { get; set; }
}