using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared.Models
{
    public class Boardgame : IProduct
    {
        public string ArtNr { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAge { get; set; }
        public int PlayTime { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string ImageText { get; set; }
        public int Stock { get; set; }
    }
}
