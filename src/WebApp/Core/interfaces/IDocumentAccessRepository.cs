using Core.Utils;

namespace Core.interfaces;

public interface IDocumentAccessRepository
{
    Task<bool> IsAuthorAsync(Guid documentId, Guid accountId);
}