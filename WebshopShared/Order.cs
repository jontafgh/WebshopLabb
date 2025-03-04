using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class OrderDto
    {
        public PersonDto Customer { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderLineDto> OrderLines { get; set; } = new List<OrderLineDto>();
    }

    public class OrderLineDto
    {
        public string ArtNr { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
