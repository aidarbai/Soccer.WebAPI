namespace Soccer.BLL.DTOs
{
    public class ResponseImportDTO<T> where T : class
    {
        public Paging? Paging { get; set; }
        public List<T> Response { get; set; } = null!;
    }

    public class Paging
    {
        public int Total { get; set; }
    }
}
