using Core.Models;

namespace API.Contracts;

public class SetPermissionRequest
{
    public PermissionType PermissionType { get; set; }
    public string? Email { get; set; }
}