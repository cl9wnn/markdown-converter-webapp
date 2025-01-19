namespace Persistence.Entities;

public class AccountEntity
{
    public Guid AccountId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? PasswordHash { get; set; }
    public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();

}