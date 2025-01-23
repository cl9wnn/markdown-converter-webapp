using Core.Models;

namespace API.Contracts;

public class SetPermissionRequest
{
    public AccountPermission Permission { get; set; }
    public string? Email { get; set; }
}