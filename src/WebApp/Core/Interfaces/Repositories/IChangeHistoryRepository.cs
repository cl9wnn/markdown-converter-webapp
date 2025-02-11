using Core.Models;
using Core.Utils;

namespace Core.Interfaces.Repositories;

public interface IChangeHistoryRepository
{
    Task<Result> LogAsync(Guid documentId, Guid? accountId, string contentHash);
    Task<Result<List<ChangeRecord>>> GetChangesByDocumentIdAsync(Guid documentId);
    Task<Result> DeleteByDocumentIdAsync(Guid documentId);
    Task<Result<ChangeRecord>> GetLastChangeRecord(Guid documentId);
}