using Core.Enums;
using Core.Models;
using Core.Utils;

namespace API.Contracts;

public class SetPermissionRequest
{
    public PermissionType PermissionType { get; set; }
    public string? Email { get; set; }
}