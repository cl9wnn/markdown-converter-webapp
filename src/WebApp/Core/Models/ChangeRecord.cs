namespace Core.Models;

public class ChangeRecord
{
    public Guid DocumentId { get; set; }
    public Guid? AccountId { get; set; }
    public DateTime Date { get; set; }
    public string ContentHash { get; set; }

    private ChangeRecord(Guid documentId, Guid? accountId, DateTime date, string contentHash)
    {
        DocumentId = documentId;
        AccountId = accountId;
        Date = date;
        ContentHash = contentHash;
    }

    public static ChangeRecord Create(Guid documentId, Guid? accountId, DateTime date, string contentHash)
    {
        return new ChangeRecord(documentId, accountId, date, contentHash);
    }
}