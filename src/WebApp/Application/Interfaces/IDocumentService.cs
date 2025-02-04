using Application.Models;
using Core.Models;
using Core.Utils;
namespace Application.Interfaces.Services;

public interface IDocumentsService
{
    Task<Result> CreateProjectAsync(Guid? accountId, string name);
    Task<Result<ICollection<Document>>> GetUserDocumentsAsync(Guid? accountId);
    Task<Result<DocumentDto>> GetDocumentAsync(Guid documentId);
    Task<Result<string>> RenameProjectAsync(Guid documentId, string newName);
    Task<Result> DeleteProjectAsync(Guid documentId);
    Task<bool> DoesDocumentExistAsync(Guid documentId);

}