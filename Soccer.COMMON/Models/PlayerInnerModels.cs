namespace Soccer.COMMON.Models
{
    public static class PlayerInnerModels
    {
        public class Birth
        {
            public string Date { get; set; } = null!;

            public string Place { get; set; } = null!;

            public string Country { get; set; } = null!;
        }

        public class Games
        {
            public int Appearences { get; set; }

            public int Lineups { get; set; }

            public int Minutes { get; set; }

            public string Position { get; set; } = null!;

            public string? Rating { get; set; }

            public bool Captain { get; set; }
        }

        public class Substitutes
        {
            public int In { get; set; }
            public int Out { get; set; }
            public int Bench { get; set; }
        }

        public class Shots
        {
            public int Total { get; set; }
            public int On { get; set; }
        }

        public class Goals
        {
            public int Total { get; set; }
            public int Conceded { get; set; }
            public int Assists { get; set; }
            public int Saves { get; set; }
        }

        public class Passes
        {
            public int Total { get; set; }
            public int Key { get; set; }
            public int Accuracy { get; set; }
        }

        public class Tackles
        {
            public int Total { get; set; }
            public int Blocks { get; set; }
            public int Interceptions { get; set; }
        }

        public class Duels
        {
            public int Total { get; set; }
            public int Won { get; set; }
        }

        public class Dribbles
        {
            public int Attempts { get; set; }
            public int Success { get; set; }
        }

        public class Fouls
        {
            public int Drawn { get; set; }
            public int Committed { get; set; }
        }

        public class Cards
        {
            public int Yellow { get; set; }
            public int Yellowred { get; set; }
            public int Red { get; set; }
        }

        public class Penalty
        {
            public int Scored { get; set; }
            public int Missed { get; set; }
            public int Saved { get; set; }
        }
    }
}
