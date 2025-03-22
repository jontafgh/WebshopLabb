using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebshopBackend.Models;

public class WebshopUser : IdentityUser
{
    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? LastName { get; set; }

    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    public Cart? Cart { get; set; }
}