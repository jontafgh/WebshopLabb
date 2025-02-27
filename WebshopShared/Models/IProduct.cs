using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared.Models
{
    public interface IProduct
    {
        public string ArtNr { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string ImageText { get; set; }
        public int Stock { get; set; }
    }
}
