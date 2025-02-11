using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentsRepository(WebDbContext dbContext): IDocumentsRepository
{
    public async Task<Result<Guid>> CreateAsync(Guid? accountId, string name)
    {
        var accountEntity = await dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        
        if (accountEntity == null)
            return Result<Guid>.Failure("Account not found");

        var documentEntity = new DocumentEntity
        {
            DocumentId = Guid.NewGuid(),
            AuthorId = accountId,
            Author = accountEntity!,
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
        
        await dbContext.Documents.AddAsync(documentEntity);
        await dbContext.SaveChangesAsync();
        
        return Result<Guid>.Success(documentEntity.DocumentId);
    }

    public async Task<Result> DeleteAsync(Guid documentId)
    {
        var deleted = await dbContext.Documents
            .Where(d => d.DocumentId == documentId)
            .ExecuteDeleteAsync();
        
        return deleted > 0
            ? Result.Success()
            : Result.Failure("Document not found");
    }
    
    public async Task<Result<Document>> GetAsync(Guid documentId)
    {
        var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId)!;
        
        if (documentEntity == null)
            return Result<Document>.Failure("Document not found")!;

        var document = Document.Create(documentEntity.DocumentId, documentEntity.AuthorId, documentEntity.Name, documentEntity.CreatedAt);

        return Result<Document>.Success(document);
    }
    
    public async Task<Result<ICollection<Document>>> GetAllByIdAsync(Guid? accountId)
    {
        var documentEntities = await dbContext.Documents
            .Where(d => d.AuthorId == accountId)
            .ToListAsync();
        
        var documents = documentEntities.Select(documentEntity =>
                Document.Create(
                    documentEntity.DocumentId,
                    documentEntity.AuthorId,
                    documentEntity.Name,
                    documentEntity.CreatedAt
                    )).ToList();
        
        return Result<ICollection<Document>>.Success(documents);
    }
    
    public async Task<Result<string>> RenameAsync(Guid documentId, string newName)
    {
        var updated = await dbContext.Documents
            .Where(d => d.DocumentId == documentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.Name, newName));

        return updated > 0 
            ? Result<string>.Success(newName) 
            : Result<string>.Failure("Document not found")!;
    }
    
    public async Task<bool> IsExistsByIdAsync(Guid documentId)
    {
       return await dbContext.Documents.AnyAsync(a => a.DocumentId == documentId);
    }

    public async Task<Result<string>> GetAuthorNameAsync(Guid documentId)
    {
        var documentEntity = await dbContext.Documents.Include(documentEntity => documentEntity.Author).FirstOrDefaultAsync(d => d.DocumentId == documentId);
        
        if (documentEntity == null)
            return Result<string>.Failure("Document not found")!;
        
        var author = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.AccountId == documentEntity.AuthorId);
        
        return Result<string>.Success(author!.FirstName!);
    }
}