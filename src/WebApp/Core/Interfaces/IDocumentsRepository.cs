using Core.Models;
using Core.Utils;

namespace Core.Interfaces;

public interface IDocumentsRepository
{
    Task<Result<Guid>> CreateAsync(Guid? accountId, string name);
    Task<Result<string>> RenameAsync(Guid documentId, string newName);
    Task<Result<Document>> GetAsync(Guid documentId);
    Task<Result<ICollection<Document>>> GetAllByIdAsync(Guid? accountId);
    Task<Result> DeleteAsync(Guid documentId);
    Task<bool> IsExistsByIdAsync(Guid documentId);
    Task<Result<string>> GetAuthorNameAsync(Guid documentId);

}