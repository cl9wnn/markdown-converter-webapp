namespace Persistence.Entities;

public class DocumentShareEntity
{
    public Guid DocumentId { get; set; }
    public Guid AccountId { get; set; }
    public int PermissionId { get; set; }
    
    public DocumentEntity Document { get; set; }
    public AccountEntity Account { get; set; }
    public PermissionEntity Permission { get; set; }
}