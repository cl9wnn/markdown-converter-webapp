using Application.Models;
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

    public async Task<Result<ICollection<Document>>> GetUserDocumentsAsync(Guid? accountId)
    {
        var getResult = await documentRepository.GetDocumentsAsync(accountId);
        
        return getResult.IsSuccess
            ? Result<ICollection<Document>>.Success(getResult.Data)
            : Result<ICollection<Document>>.Failure(getResult.ErrorMessage!)!;
    }
    
    public async Task<Result<DocumentDto>> GetDocumentAsync(Guid documentId)
    {
        var authorNameResult = await documentRepository.GetAuthorNameAsync(documentId);
        
        if (!authorNameResult.IsSuccess)
            return Result<DocumentDto>.Failure(authorNameResult.ErrorMessage!)!;
        
        var getResult = await documentRepository.GetDocumentAsync(documentId);

        var documentDto = new DocumentDto
        {
            DocumentId = getResult.Data.DocumentId,
            AuthorName = authorNameResult.Data,
            Name = getResult.Data.Name,
        };

        return getResult.IsSuccess
            ? Result<DocumentDto>.Success(documentDto)
            : Result<DocumentDto>.Failure(getResult.ErrorMessage!)!;
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
        return await documentRepository.IsDocumentExistsByIdAsync(documentId);
    }
    
}