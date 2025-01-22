using Core.interfaces;
using Core.Utils;

namespace Application.Services;

public class DocumentAccessService(IDocumentAccessRepository documentAccessRepository)
{
    public async Task<bool> IsAuthorAsync(Guid documentId, Guid accountId)
    {
        return await documentAccessRepository.IsAuthorAsync(documentId, accountId);
    }
}