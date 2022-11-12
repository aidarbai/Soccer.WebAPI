namespace Soccer.BLL.DTOs
{
    public class LeagueDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Country { get; set; }
        public string? Flag { get; set; }
    }
}
