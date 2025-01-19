using Core.Models;
using Core.Utils;
namespace Core.interfaces;

public interface IDocumentsRepository
{
    Task<Result<Guid>> CreateDocumentAsync(Guid userId, string name);
    Task<Result> RenameDocumentAsync(Guid userId, string name);
    Task<Result<Document>> GetDocumentAsync(Guid userId, string name);
    Task<Result<string>> DeleteDocumentAsync(Guid documentId);
}