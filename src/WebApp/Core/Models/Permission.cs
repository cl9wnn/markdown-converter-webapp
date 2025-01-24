namespace Core.Models;

public class Permission
{
    public int PermissionId { get; set; }
    public Guid AccountId { get; set; }
    public Guid DocumentId { get; set; }
}