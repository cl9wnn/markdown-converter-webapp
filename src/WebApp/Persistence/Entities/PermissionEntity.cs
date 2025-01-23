namespace Persistence.Entities;

public class PermissionEntity
{
    public int PermissionId { get; set; }
    public string Name { get; set; }
    public ICollection<DocumentShareEntity> DocumentShares { get; set; } = new List<DocumentShareEntity>();
}