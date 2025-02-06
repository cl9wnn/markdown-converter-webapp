using Application.Models;
using Core.Models;
using Core.Utils;

namespace Application.Interfaces;

public interface IDocumentsService
{
    Task<Result> CreateDocumentAsync(Guid? accountId, string name);
    Task<Result<ICollection<Document>>> GetUserDocumentsAsync(Guid? accountId);
    Task<Result<DocumentDto>> GetDocumentAsync(Guid documentId);
    Task<Result<string>> RenameDocumentAsync(Guid documentId, Guid? accountId, string newName);
    Task<Result> DeleteDocumentAsync(Guid documentId, Guid? accountId);
    Task<bool> IsDocumentExistsAsync(Guid documentId);
}