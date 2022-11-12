namespace Soccer.BLL.DTOs
{
    public class ResponseLeagueImportDTO
    {
        public LeagueImportDTO League { get; set; } = null!;
        public CountryImportDTO Country{ get; set; } = null!;
    }
}
