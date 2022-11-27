using Soccer.DAL.Attributes;
using static Soccer.COMMON.Models.PlayerInnerModels;

namespace Soccer.DAL.Models
{
    [BsonCollection("players")]
    public class Player : Document
    {
        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        //public int Age { get; set; }

        public Birth Birth { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string Height { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public bool Injured { get; set; }

        public string Photo { get; set; } = null!;

        public List<Statistic> Statistics { get; set; } = new();
    }
}
