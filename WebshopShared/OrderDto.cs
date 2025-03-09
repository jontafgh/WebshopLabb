using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class PlaceOrderDto
    {
        public List<CartItemDto> CartItems { get; set; } = [];
    }
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CartItemDto> CartItems { get; set; } = [];
    }
}
