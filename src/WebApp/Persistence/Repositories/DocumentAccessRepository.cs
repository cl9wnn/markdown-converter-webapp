using Core.interfaces;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

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
}