using Application.Models;
using Core.Utils;

namespace Application.Interfaces.Services;

public interface IDocumentAccessService
{
    Task<bool> IsAuthorAsync(Guid documentId, Guid accountId);
    Task<Result> SetUserPermissionAsync(PermissionType permissionType, Guid documentId, Guid accountId);
    Task<Result> ClearPermissionAsync(Guid documentId, string email);
    Task<Result<ICollection<DocumentPermissionDto>>> GetDocumentPermissionListAsync(Guid documentId);
    Task<int> GetUserPermissionAsync(Guid documentId, Guid accountId);
}