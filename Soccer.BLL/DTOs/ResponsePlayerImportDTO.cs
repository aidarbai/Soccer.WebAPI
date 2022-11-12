namespace Soccer.BLL.DTOs
{
    public class ResponsePlayerImportDTO
    {
        public PlayerDTO Player { get; set; } = null!;
        public List<StatisticDTO> Statistics { get; set; } = null!;
    }
}
