using Core.Models;
using Core.Utils;

namespace Core.Interfaces;

public interface IChangeHistoryRepository
{
    Task<Result> LogAsync(Guid documentId, Guid? accountId);
    Task<Result<List<ChangeRecord>>> GetChangesByDocumentIdAsync(Guid documentId);
    Task<Result> DeleteByDocumentIdAsync(Guid documentId);
}