namespace Core.Models;

public class ChangeRecord
{
    public Guid DocumentId { get; set; }
    public Guid? AccountId { get; set; }
    public DateTime Date { get; set; }

    private ChangeRecord(Guid documentId, Guid? accountId, DateTime date)
    {
        DocumentId = documentId;
        AccountId = accountId;
        Date = date;
    }

    public static ChangeRecord Create(Guid documentId, Guid? accountId, DateTime date)
    {
        return new ChangeRecord(documentId, accountId, date);
    }
}