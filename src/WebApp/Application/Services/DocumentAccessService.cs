using Application.Interfaces.Services;
using Application.Models;
using Core.Interfaces.Repositories;
using Core.Utils;
namespace Application.Services;

public class DocumentAccessService(IDocumentAccessRepository documentAccessRepository, IAccountRepository accountRepository): IDocumentAccessService
{
    public async Task<bool> IsAuthorAsync(Guid documentId, Guid accountId)
    {
        return await documentAccessRepository.IsAuthorAsync(documentId, accountId);
    }
    
    public async Task<Result> SetUserPermissionAsync(PermissionType permissionType, Guid documentId, Guid accountId)
    {
        var permissionId = (int)permissionType;
        
        var setResult = await documentAccessRepository.AddDocumentShareAsync(permissionId, documentId, accountId);
        
        return setResult.IsSuccess
            ? Result.Success()
            : Result.Failure(setResult.ErrorMessage!);
    }

    public async Task<Result> ClearPermissionAsync(Guid documentId, string email)
    {
        var accountResult = await accountRepository.GetByEmailAsync(email);
        
        if (!accountResult.IsSuccess)
            return Result.Failure(accountResult.ErrorMessage!);
        
        var clearResult = await documentAccessRepository.ClearPermissionsAsync(documentId, accountResult!.Data!.AccountId);
        
        return clearResult.IsSuccess
            ? Result.Success()
            : Result.Failure(clearResult.ErrorMessage!);
    }

    public async Task<Result<ICollection<DocumentPermissionDto>>> GetDocumentPermissionListAsync(Guid documentId)
    {
        var getPermissionsResult = await documentAccessRepository.GetAllDocumentPermissionsAsync(documentId);
        
        if (!getPermissionsResult.IsSuccess)
            return Result<ICollection<DocumentPermissionDto>>.Failure(getPermissionsResult.ErrorMessage!)!;
        
        var permissions = new List<DocumentPermissionDto>();

        foreach (var p in getPermissionsResult.Data)
        {
            var accountResult = await accountRepository.GetByAccountIdAsync(p.AccountId);

            if (!accountResult.IsSuccess)
                continue; 

            permissions.Add(new DocumentPermissionDto
            {
                DocumentId = p.DocumentId,
                PermissionName = Enum.GetName(typeof(PermissionType), p.PermissionId)!,
                Email = accountResult.Data.Email!
            });
        }
        
        return Result<ICollection<DocumentPermissionDto>>.Success(permissions);
    }

    public async Task<int> GetUserPermissionAsync(Guid documentId, Guid accountId)
    {
        return await documentAccessRepository.GetUserPermissionAsync(documentId, accountId);
    }
}