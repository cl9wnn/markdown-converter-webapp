namespace Core.Models;

public class Document
{
    public Guid DocumentId { get; set; }
    public Guid? AuthorId { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }

    private Document(Guid documentId, Guid? accountId, string? name, DateTime createdAt)
    {
        DocumentId = documentId;
        AuthorId = accountId;
        Name = name;
        CreatedAt = createdAt;
    }

    public static Document Create(Guid documentId, Guid? accountId, string? name, DateTime createdAt)
    {
        return new Document(documentId, accountId, name, createdAt);
    }
}