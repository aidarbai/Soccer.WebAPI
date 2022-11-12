namespace Soccer.BLL.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Founded { get; set; }
        public string? Logo { get; set; }
        public string? Venue { get; set; }
        public string? City { get; set; }
    }
}
