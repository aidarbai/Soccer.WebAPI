namespace Soccer.COMMON.ViewModels
{
    public class PaginatedResponse<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public long ItemsCount { get; set; }
        public List<T> Results { get; set; } = null!;
    }
}
