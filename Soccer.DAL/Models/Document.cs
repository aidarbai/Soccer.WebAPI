namespace Soccer.DAL.Models
{
    public abstract class Document
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
