using Core.interfaces;
using Core.Models;
using Core.Utils;

namespace Application.Services;

public class DocumentsService(IDocumentsRepository documentRepository, MinioService minIoService)
{

    public async Task<Result> CreateProjectAsync(Guid? accountId, string name)
    {
        var ctx = new CancellationTokenSource();

        var createResult = await documentRepository.CreateDocumentAsync(accountId, name);
        
        if (!createResult.IsSuccess)
            return Result.Failure(createResult.ErrorMessage!);
        
        var fileName = $"{createResult.Data}.md";
        const string content = "_Write something in markdown..._";
        
        try
        {
            await minIoService.UploadMarkdownTextAsync(content, fileName, ctx.Token);
        }
        catch (Exception ex)
        {
            await documentRepository.DeleteDocumentAsync(createResult.Data);
            return Result.Failure(ex.Message);
        }
        return Result.Success();
    }

    public async Task<Result<ICollection<Document>>> GetUserProjectsAsync(Guid? accountId)
    {
        var getResult = await documentRepository.GetDocumentsAsync(accountId);
        
        return getResult.IsSuccess
            ? Result<ICollection<Document>>.Success(getResult.Data)
            : Result<ICollection<Document>>.Failure(getResult.ErrorMessage!)!;
    }
    
    public async Task<Result<Document>> GetProjectAsync(Guid documentId)
    {
        var getResult = await documentRepository.GetDocumentAsync(documentId);
        
        return getResult.IsSuccess
            ? Result<Document>.Success(getResult.Data)
            : Result<Document>.Failure(getResult.ErrorMessage!)!;
    }
    
    public async Task<Result<string>> RenameProjectAsync(Guid documentId, string newName)
    {
        var renameResult = await documentRepository.RenameDocumentAsync(documentId, newName);
        
        return renameResult.IsSuccess
            ? Result<string>.Success(renameResult.Data)
            : Result<string>.Failure(renameResult.ErrorMessage!)!;
    }
    
    public async Task<Result<string>> DeleteProjectAsync(Guid documentId)
    {
        var ctx = new CancellationTokenSource();
        
        var deleteResult = await documentRepository.DeleteDocumentAsync(documentId);
        
        var fileName = $"{documentId}.md"; 

        try
        {
            await minIoService.DeleteFileAsync(fileName, ctx.Token);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message)!;
        }
        
        return deleteResult.IsSuccess
            ? Result<string>.Success(deleteResult.Data)
            : Result<string>.Failure(deleteResult.ErrorMessage!)!;
    }
    
    public async Task<bool> DoesDocumentExistAsync(Guid documentId)
    {
        return await documentRepository.IsDocumentExistsById(documentId);
    }
    
}