using Core.Models;
using Core.Utils;
namespace Core.interfaces;

public interface IDocumentsRepository
{
    Task<Result<Guid>> CreateDocumentAsync(Guid? accountId, string name);
    Task<Result<string>> RenameDocumentAsync(Guid documentId, string newName);
    Task<Result<Document>> GetDocumentAsync(Guid documentId);
    Task<Result<ICollection<Document>>> GetDocumentsAsync(Guid? accountId);
    Task<Result<string>> DeleteDocumentAsync(Guid documentId);
    Task<bool> IsDocumentExistsById(Guid documentId);
    Task<Result<string>> GetAuthorNameAsync(Guid documentId);

}