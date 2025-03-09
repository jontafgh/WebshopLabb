using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class CreateCartDto
    {
        public string UserId { get; set; } = null!;
    }
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public UserDetailsDto UserDetails { get; set; } = null!;
        public List<CartItemDto> CartItems { get; set; } = [];
    }
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public string Name { get; set; }
        public string ArtNr { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
