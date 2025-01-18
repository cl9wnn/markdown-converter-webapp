using Core.Utils;
using Markdown.Interfaces;

namespace Application.Services;
using Markdown;
public class MdService(IMarkdownProcessor markdownProcessor)
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
}