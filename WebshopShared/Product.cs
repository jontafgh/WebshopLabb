using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class Product
    {
        public int ID { get; set; }
        public string ArtNr { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAge { get; set; }
        public int PlayTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Image { get; set; }
        public string ImageText { get; set; }
        public int Stock { get; set; }
    }
}
