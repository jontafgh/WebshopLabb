using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebshopShared
{
    public class UserDetailsDto
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressDto? Address { get; set; }
    }

    public class AddressDto
    {
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; } 
        public string? Country { get; set; } 
    }
}
