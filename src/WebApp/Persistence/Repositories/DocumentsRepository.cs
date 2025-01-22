using Core.interfaces;
using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class DocumentsRepository(WebDbContext dbContext): IDocumentsRepository
{
    public async Task<Result<Guid>> CreateDocumentAsync(Guid? accountId, string name)
    {
        if (accountId == null)
            return Result<Guid>.Failure("Account not found");
        
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

    public async Task<Result<string>> DeleteDocumentAsync(Guid documentId)
    { 
        var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);

        if (documentEntity == null)
            return Result<string>.Failure("Document not found")!;
        
        dbContext.Documents.Remove(documentEntity!);
        await dbContext.SaveChangesAsync();
        
        return Result<string>.Success(documentEntity.Name!);
    }
    
    public async Task<Result<Document>> GetDocumentAsync(Guid documentId)
    {
        var documentEntity = await dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId)!;
        
        if (documentEntity == null)
            return Result<Document>.Failure("Document not found")!;

        var document = new Document
        {
            DocumentId = documentEntity.DocumentId,
            AuthorId = documentEntity.AuthorId,
            Name = documentEntity.Name,
            CreatedAt = documentEntity.CreatedAt,
        };

        return Result<Document>.Success(document);
    }
    
    public async Task<Result<ICollection<Document>>> GetDocumentsAsync(Guid? accountId)
    {
        if (accountId == null)
            return Result<ICollection<Document>>.Failure("Account not found")!;
        
        var documentEntities = await dbContext.Documents
            .Where(d => d.AuthorId == accountId)
            .ToListAsync();
        
        
        var documents = documentEntities.Select(documentEntity => new Document
        {
            DocumentId = documentEntity.DocumentId,
            AuthorId = documentEntity.AuthorId,
            Name = documentEntity.Name,
            CreatedAt = documentEntity.CreatedAt,
        }).ToList();
        
        return Result<ICollection<Document>>.Success(documents);
    }
    
    public async Task<Result<string>> RenameDocumentAsync(Guid documentId, string newName)
    {
        var documentEntity = await  dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
        
        if (documentEntity == null)
            return Result<string>.Failure("Document not found")!;
        
        documentEntity.Name = newName;
        await dbContext.SaveChangesAsync();
        
        return Result<string>.Success(newName);
    }
    
    public async Task<bool> IsDocumentExistsById(Guid documentId)
    {
       return await dbContext.Documents.AnyAsync(a => a.DocumentId == documentId);
    }
}