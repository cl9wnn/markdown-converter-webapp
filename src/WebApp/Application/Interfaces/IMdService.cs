using Core.Utils;

namespace Application.Interfaces;

public interface IMdService
{
    Task<Result<string>> ConvertToHtmlAsync(string rawMarkdown);
    Task<Result> SaveMarkdownAsync(Guid documentId, Guid? accountId, string content);
    Task<Result<string>> GetMarkdownAsync(Guid documentId);
}