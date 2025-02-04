namespace Core.Models;

public class Permission
{
    public int PermissionId { get; set; }
    public Guid AccountId { get; set; }
    public Guid DocumentId { get; set; }

    private Permission(int permissionId, Guid accountId, Guid documentId)
    {
        PermissionId = permissionId;
        AccountId = accountId;
        DocumentId = documentId;
    }

    public static Permission Create(int permissionId, Guid accountId, Guid documentId)
    {
        return new Permission(permissionId, accountId, documentId);
    }
}