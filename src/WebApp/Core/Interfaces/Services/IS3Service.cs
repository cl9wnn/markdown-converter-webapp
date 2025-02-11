using Core.Utils;

namespace Core.Interfaces.Services;

public interface IS3Service
{
    Task<Result<string>> UploadMarkdownTextAsync(string markdownContent, string objectName,
        CancellationToken cancellationToken);

    Task<string?> GetMarkdownTextAsync(string objectName, CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);
}