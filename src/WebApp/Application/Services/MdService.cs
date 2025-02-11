using Application.Interfaces;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Utils;
using Markdown.Interfaces;

namespace Application.Services;
public class MdService(IMarkdownProcessor markdownProcessor, IChangeHistoryService historyService, IS3Service minIoService): IMdService
{
    public async Task<Result<string>> ConvertToHtmlAsync(string rawMarkdown)
    {
        try
        {
            var htmlString = await Task.Run(() => markdownProcessor.ConvertToHtmlFromString(rawMarkdown));
            return Result<string>.Success(htmlString);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message!)!;
        }
    }
    
    public async Task<Result> SaveMarkdownAsync(Guid documentId, Guid? accountId, string content)
    {
        using var ctx = new CancellationTokenSource();
        var fileName = $"{documentId}.md"; 

        var uploadResult = await minIoService.UploadMarkdownTextAsync(content, fileName, ctx.Token);

        if (!uploadResult.IsSuccess)
            return Result.Failure(uploadResult.ErrorMessage!);
            
        var logResult = await historyService.LogChangeAsync(documentId, accountId, content);
        
        if (!logResult.IsSuccess)
            return Result.Failure(logResult.ErrorMessage!);
     
        return Result.Success();
    }

    public async Task<Result<string>> GetMarkdownAsync(Guid documentId)
    {
        var ctx = new CancellationTokenSource();
        var fileName = $"{documentId}.md";

        try
        {
            var content = await minIoService.GetMarkdownTextAsync(fileName, ctx.Token);
            return Result<string>.Success(content);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message)!;
        }
    }
}