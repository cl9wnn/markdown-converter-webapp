using Core.interfaces;
using Core.Models;
using Core.Utils;

namespace Persistence.Repositories;

public class DocumentsRepository: IDocumentsRepository
{
    public Task<Result<Guid>> CreateDocumentAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RenameDocumentAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Document>> GetDocumentAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteDocumentAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}