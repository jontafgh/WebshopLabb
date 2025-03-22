using System.ComponentModel.DataAnnotations.Schema;
using WebshopShared;

namespace WebshopBackend.Models
{
    public class Boardgame
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(3000)")]
        public string Description { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAge { get; set; }
        public int PlayTime { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public Publisher? Publisher { get; set; }
    }

    public class Publisher
    {
        public int Id { get; set; }
        public int? BoardgameId { get; set; }
        public Boardgame? Boardgame { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
    }
}
