using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebshopShared
{
    public class Person
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [ValidateComplexType]
        public Address Address { get; set; } = new Address();
    }

    public class Address
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; }
    }
}
