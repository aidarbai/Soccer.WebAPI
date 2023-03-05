using Soccer.DAL.Attributes;

namespace Soccer.DAL.Models
{
    [BsonCollection("teams")]
    public class Team : Document
    {
        public int Founded { get; set; }
        public string? Logo { get; set; }
        public string? Venue { get; set; }
        public string? City { get; set; }
        public string? League { get; set; }
        public string? CardColor { get; set; }
    }
}
