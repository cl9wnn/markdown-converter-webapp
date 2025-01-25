using Core.Models;
using Core.Utils;

namespace Core.Interfaces.Repositories;

public interface IDocumentsRepository
{
    Task<Result<Guid>> CreateDocumentAsync(Guid? accountId, string name);
    Task<Result<string>> RenameDocumentAsync(Guid documentId, string newName);
    Task<Result<Document>> GetDocumentAsync(Guid documentId);
    Task<Result<ICollection<Document>>> GetDocumentsAsync(Guid? accountId);
    Task<Result<string>> DeleteDocumentAsync(Guid documentId);
    Task<bool> IsDocumentExistsByIdAsync(Guid documentId);
    Task<Result<string>> GetAuthorNameAsync(Guid documentId);

}