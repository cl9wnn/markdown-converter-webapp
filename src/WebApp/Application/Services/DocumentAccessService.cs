using Core.interfaces;
using Core.Models;
using Core.Utils;

namespace Application.Services;

public class DocumentAccessService(IDocumentAccessRepository documentAccessRepository)
{
    public async Task<bool> IsAuthorAsync(Guid documentId, Guid accountId)
    {
        return await documentAccessRepository.IsAuthorAsync(documentId, accountId);
    }
    
    public async Task<Result> SetUserPermissionAsync(AccountPermission permission, Guid documentId, Guid accountId)
    {
        var permissionId = (int)permission;
        Result setResult;
        
        if (permission == AccountPermission.NoAccess)
            setResult = await documentAccessRepository.ClearPermissionsAsync(documentId, accountId);
        else
            setResult = await documentAccessRepository.AddDocumentShareAsync(permissionId, documentId, accountId);
        
        return setResult.IsSuccess
            ? Result.Success()
            : Result.Failure(setResult.ErrorMessage!);
    }
}