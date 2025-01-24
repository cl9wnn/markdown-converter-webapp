namespace Application.Models;

public class DocumentPermissionDto
{
    public Guid DocumentId { get; set; }
    public string Email { get; set; }
    public string PermissionName { get; set; }
}