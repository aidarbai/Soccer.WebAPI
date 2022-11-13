
namespace Soccer.BLL.DTOs
{
    public class ResponsePlayerImportDTO
    {
        public PlayerImportDTO Player { get; set; } = null!;
        public List<StatisticImportDTO> Statistics { get; set; } = null!;
    }
}
