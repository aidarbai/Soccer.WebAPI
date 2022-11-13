namespace Soccer.COMMON.ViewModels
{
    public class LeagueVM
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Country { get; set; }
        public string? Flag { get; set; }
    }
}
