using static Soccer.COMMON.Models.PlayerInnerModels;

namespace Soccer.COMMON.ViewModels
{
    public class StatisticVM
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
