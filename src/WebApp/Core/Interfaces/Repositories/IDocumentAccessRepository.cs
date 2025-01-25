using Core.Models;
using Core.Utils;

namespace Core.Interfaces.Repositories;

public interface IDocumentAccessRepository
{
    Task<bool> IsAuthorAsync(Guid documentId, Guid accountId);
    Task<Result> AddDocumentShareAsync(int permissionId, Guid documentId, Guid accountId);
    Task<Result> ClearPermissionsAsync(Guid documentId, Guid accountId);
    Task<Result<ICollection<Permission>>> GetAllDocumentPermissionsAsync(Guid documentId);
    Task<int> GetUserPermissionAsync(Guid documentId, Guid accountId);

}