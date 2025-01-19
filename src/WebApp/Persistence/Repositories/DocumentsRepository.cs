using Core.interfaces;
using Core.Models;
using Core.Utils;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentsRepository(WebDbContext dbContext): IDocumentsRepository
{
    public async Task<Result<Guid>> CreateDocumentAsync(Guid userId, string name)
    {
        var userEntity = dbContext.Accounts.FirstOrDefault(a => a.AccountId == userId);
        
        if (userEntity == null)
            return Result<Guid>.Failure("User not found");

        var documentEntity = new DocumentEntity
        {
            DocumentId = Guid.NewGuid(),
            AuthorId = userId,
            Author = userEntity!,
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
        
        await dbContext.Documents.AddAsync(documentEntity);
        await dbContext.SaveChangesAsync();
        
        return Result<Guid>.Success(documentEntity.DocumentId);
    }

    public async Task<Result<string>> DeleteDocumentAsync(Guid documentId)
    { 
        var documentEntity = dbContext.Documents.FirstOrDefault(d => d.DocumentId == documentId);

        if (documentEntity == null)
            return Result<string>.Failure("Document not found")!;
        
        dbContext.Documents.Remove(documentEntity!);
        await dbContext.SaveChangesAsync();
        
        return Result<string>.Success(documentEntity.Name!);
    }
    
    public async Task<Result> RenameDocumentAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Document>> GetDocumentAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }
}