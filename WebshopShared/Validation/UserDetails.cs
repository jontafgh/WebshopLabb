using System.ComponentModel.DataAnnotations;

namespace WebshopShared.Validation
{
    public class UserDetails
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [ValidateComplexType]
        public AddressDetails Address { get; set; } = new();
    }

    public class AddressDetails
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Street { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Country { get; set; } = null!;
    }
}
