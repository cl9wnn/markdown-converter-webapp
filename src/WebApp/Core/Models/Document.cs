namespace Core.Models;

public class Document
{
    public Guid DocumentId { get; set; }
    public Guid AuthorId { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}