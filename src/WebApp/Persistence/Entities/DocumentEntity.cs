namespace Persistence.Entities;

public class DocumentEntity
{
    public Guid DocumentId { get; set; }
    public Guid? AuthorId { get; set; }
    public AccountEntity Author { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}