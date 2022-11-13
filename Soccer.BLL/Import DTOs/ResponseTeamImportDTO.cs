namespace Soccer.BLL.DTOs
{
    public class ResponseTeamImportDTO
    {
        public TeamImportDTO Team { get; set; } = null!;
        public VenueImportDTO Venue{ get; set; } = null!;
    }
}
