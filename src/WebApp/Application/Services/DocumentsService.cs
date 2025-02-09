using Application.Interfaces;
using Application.Models;
using Core.Interfaces;
using Core.Models;
using Core.Utils;
namespace Application.Services;

public class DocumentsService(IDocumentsRepository documentRepository, MinioService minIoService,
    RedisCacheService cacheService, IChangeHistoryService historyService): IDocumentsService
{
    public async Task<Result> CreateDocumentAsync(Guid? accountId, string name)
    {
        var ctx = new CancellationTokenSource();

        var createResult = await documentRepository.CreateAsync(accountId, name);
        
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
            await documentRepository.DeleteAsync(createResult.Data);
            return Result.Failure(ex.Message);
        }
        
        await cacheService.RemoveValueAsync($"user_docs_{accountId}");

        return Result.Success();
    }

    public async Task<Result<ICollection<Document>>> GetUserDocumentsAsync(Guid? accountId)
    {
        var cacheKey = $"user_docs_{accountId}";

        var cachedDocuments = await cacheService.GetValueAsync<ICollection<Document>>(cacheKey);
        
        if (cachedDocuments is not null)
            return Result<ICollection<Document>>.Success(cachedDocuments);
        
        var getResult = await documentRepository.GetAllByIdAsync(accountId);

        if (!getResult.IsSuccess)
            return Result<ICollection<Document>>.Failure(getResult.ErrorMessage!)!;

        await cacheService.SetValueAsync(cacheKey, getResult.Data, TimeSpan.FromMinutes(10));

        return Result<ICollection<Document>>.Success(getResult.Data);
    }
    
    public async Task<Result<DocumentDto>> GetDocumentAsync(Guid documentId)
    {
        var authorNameResult = await documentRepository.GetAuthorNameAsync(documentId);
        
        if (!authorNameResult.IsSuccess)
            return Result<DocumentDto>.Failure(authorNameResult.ErrorMessage!)!;
        
        var getResult = await documentRepository.GetAsync(documentId);

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
    
    public async Task<Result<string>> RenameDocumentAsync(Guid documentId, Guid? accountId, string newName)
    {
        var renameResult = await documentRepository.RenameAsync(documentId, newName);
        
        if  (!renameResult.IsSuccess)
             Result<string>.Failure(renameResult.ErrorMessage!);
        
        await cacheService.RemoveValueAsync($"user_docs_{accountId}");

        return Result<string>.Success(renameResult.Data);
    }
    
    public async Task<Result> DeleteDocumentAsync(Guid documentId, Guid? accountId)
    {
        var ctx = new CancellationTokenSource();
        
        var deleteResult = await documentRepository.DeleteAsync(documentId);
        
        if (!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.ErrorMessage!); 
        
        var fileName = $"{documentId}.md"; 

        try
        {
            await minIoService.DeleteFileAsync(fileName, ctx.Token);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message)!;
        }
        
        if (!deleteResult.IsSuccess)
             Result.Failure(deleteResult.ErrorMessage!);

        var historyDeleteResult = await historyService.ClearChangeHistoryAsync(documentId);
        
        if (!historyDeleteResult.IsSuccess)
            return historyDeleteResult;
        
        await cacheService.RemoveValueAsync($"user_docs_{accountId}");

        return Result.Success();
    }
    
    public async Task<bool> IsDocumentExistsAsync(Guid documentId)
    {
        return await documentRepository.IsExistsByIdAsync(documentId);
    }
    
}