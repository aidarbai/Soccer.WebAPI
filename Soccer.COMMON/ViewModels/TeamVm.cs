namespace Soccer.COMMON.ViewModels
{
    public class TeamVm
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Founded { get; set; }
        public string? Logo { get; set; }
        public string? Venue { get; set; }
        public string? City { get; set; }
        public string? League { get; set; }
        public string? CardColor { get; set; }
    }
}
