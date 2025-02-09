namespace Application.Models;

public class DocumentDto
{
    public Guid DocumentId { get; set; }
    public string? AuthorName { get; set; }
    public string? Name { get; set; }
}