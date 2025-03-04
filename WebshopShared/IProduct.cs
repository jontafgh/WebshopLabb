using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public interface IProduct
    {
        public string ArtNr { get; set; } 
        public string Name { get; set; } 
        public PublisherDto Publisher { get; set; } 
        public ImageDto Image { get; set; } 
        public PriceDto Price { get; set; } 
        public StockDto Stock { get; set; } 
    }
}
