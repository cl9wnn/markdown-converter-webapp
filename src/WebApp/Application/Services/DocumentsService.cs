using Application.Models;
using Core.interfaces;
using Core.Utils;

namespace Application.Services;

public class DocumentsService(IDocumentsRepository documentRepository, MinioService minIoService)
{
    public async Task<Result> CreateAsync(Guid userId, string name, string content)
    {
        var ctx = new CancellationTokenSource();
        
        var createResult = await documentRepository.CreateDocumentAsync(userId, name);

        if (!createResult.IsSuccess)
            return Result.Failure(createResult.ErrorMessage!);

        var fileName = $"{createResult.Data}.md"; 

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
    
}