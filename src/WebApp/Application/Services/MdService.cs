using Application.Interfaces.Services;
using Core.Utils;
using Markdown.Interfaces;

namespace Application.Services;
public class MdService(IMarkdownProcessor markdownProcessor, MinioService minIoService): IMdService
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
    
    public async Task<Result> SaveMarkdownAsync(Guid documentId, string content)
    {
        var ctx = new CancellationTokenSource();
        var fileName = $"{documentId}.md"; 

        try
        {
            await minIoService.UploadMarkdownTextAsync(content, fileName, ctx.Token);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
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