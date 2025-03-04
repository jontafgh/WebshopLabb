using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
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

    public class CartItemToAddDto
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemToUpdateDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
