using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentAccessRepository(WebDbContext dbContext): IDocumentAccessRepository
{
    public async Task<bool> IsAuthorAsync(Guid documentId, Guid accountId)
    {
        var document = await dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId); 
        
        if (document == null)
            return false;
        
        return document!.AuthorId == accountId;
    }

    public async Task<Result> AddDocumentShareAsync(int permissionId, Guid documentId, Guid accountId)
    {
        var accountEntity = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.AccountId == accountId);

        if (accountEntity == null)
            return Result.Failure("Account not found");
        
        var documentEntity = await dbContext.Documents
            .FirstOrDefaultAsync(d => d.DocumentId == documentId);

        if (documentEntity == null)
            return Result.Failure("Document not found");
        
        if (documentEntity.AuthorId == accountId)
            return Result.Failure("You are already owner");
        
        var existingShare = await dbContext.DocumentShares
            .FirstOrDefaultAsync(d => d.DocumentId == documentId && d.AccountId == accountId);

        if (existingShare != null)
        {
            existingShare.PermissionId = permissionId;
        }
        else
        {
            var documentShare = new DocumentShareEntity
            {
                DocumentId = documentEntity.DocumentId,
                AccountId = accountEntity.AccountId,
                PermissionId = permissionId
            };
        
            await dbContext.DocumentShares.AddAsync(documentShare);
        }
        
        await dbContext.SaveChangesAsync();
        
        return Result.Success();
    }
    
    public async Task<Result> ClearPermissionsAsync(Guid documentId, Guid accountId)
    {
        var deletedCount = await dbContext.DocumentShares
            .Where(ds => ds.DocumentId == documentId && ds.AccountId == accountId)
            .ExecuteDeleteAsync();

        return deletedCount > 0 
            ? Result.Success() 
            : Result.Failure("Permissions not found");
    }

    public async Task<Result<ICollection<Permission>>> GetAllDocumentPermissionsAsync(Guid documentId)
    {
        var documentPermissions = await dbContext.DocumentShares
            .Where(dp => dp.DocumentId == documentId)
            .ToListAsync();

        var documentPermissionsList = documentPermissions.Select(dp => 
            Permission.Create(
                dp.PermissionId,
                dp. AccountId,
                dp.DocumentId
                )).ToList();
        
        return Result<ICollection<Permission>>.Success(documentPermissionsList);
    }

    public async Task<int> GetUserPermissionAsync(Guid documentId, Guid accountId)
    {
        var documentShareEntity = await dbContext.DocumentShares
            .FirstOrDefaultAsync(ds => ds.DocumentId == documentId && ds.AccountId == accountId);

        return documentShareEntity?.PermissionId ?? 0;
    }
}