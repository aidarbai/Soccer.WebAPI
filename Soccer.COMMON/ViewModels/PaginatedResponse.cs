namespace Soccer.COMMON.ViewModels
{
    public class PaginatedResponse<T>
    {
        public uint PageSize { get; set; }
        public uint PageNumber { get; set; }
        public uint TotalPages { get; set; }
        public ulong ItemsCount { get; set; }
        public List<T> Results { get; set; } = null!;
    }
}
