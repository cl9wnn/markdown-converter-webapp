﻿using Core.Utils;

namespace Application.Interfaces.Services;

public interface IMdService
{
    Task<Result<string>> ConvertToHtmlAsync(string rawMarkdown);
    Task<Result> SaveMarkdownAsync(Guid documentId, string content);
    Task<Result<string>> GetMarkdownAsync(Guid documentId);
}