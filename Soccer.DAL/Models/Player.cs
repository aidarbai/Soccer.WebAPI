using Soccer.DAL.Attributes;
using static Soccer.COMMON.Models.PlayerInnerModels;

namespace Soccer.DAL.Models
{
    [BsonCollection("players")]
    public class Player : Document
    {
        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public int Age { get; set; }

        public Birth Birth { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string Height { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public bool Injured { get; set; }

        public string Photo { get; set; } = null!;

        public List<Statistic> Statistics { get; set; } = new();
    }

    public class Statistic
    {
        public string Team { get; set; } = null!;
        public string League { get; set; } = null!;
        public Games Games { get; set; } = null!;
        public Substitutes Substitutes { get; set; } = null!;
        public Shots Shots { get; set; } = null!;
        public Goals Goals { get; set; } = null!;
        public Passes Passes { get; set; } = null!;
        public Tackles Tackles { get; set; } = null!;
        public Duels Duels { get; set; } = null!;
        public Dribbles Dribbles { get; set; } = null!;
        public Fouls Fouls { get; set; } = null!;
        public Cards Cards { get; set; } = null!;
        public Penalty Penalty { get; set; } = null!;
    }
}
