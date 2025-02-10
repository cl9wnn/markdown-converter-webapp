using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Models;
using Core.Interfaces;
using Core.Models;
using Core.Utils;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ChangeHistoryService(IChangeHistoryRepository historyRepository, IAccountService accountService, ILogger<ChangeHistoryService> logger): IChangeHistoryService
{
    
    public async Task<Result<List<ChangeRecordDto>>> GetChangeHistoryAsync(Guid documentId)
    {
        var getResult = await historyRepository.GetChangesByDocumentIdAsync(documentId);

        if (!getResult.IsSuccess)
            return Result<List<ChangeRecordDto>>.Failure(getResult.ErrorMessage!)!;

        var changes = getResult.Data.ToList(); 

        var changeRecords = new List<ChangeRecordDto>();

        foreach (var change in changes)
        {
            var accountResult = await accountService.GetAccountByIdAsync(change.AccountId);
            changeRecords.Add(new ChangeRecordDto
            {
                UserName = accountResult.IsSuccess ? accountResult.Data.FirstName : "Unknown",
                Date = change.Date
            });
        }

        return Result<List<ChangeRecordDto>>.Success(changeRecords);
    }

    public async Task<Result> LogChangeAsync(Guid documentId, Guid? accountId, string content)
    {
        var currentContentHash = ComputeHash(content);
        logger.LogInformation("HASH _____ " + currentContentHash);
        
        var getResult = await historyRepository.GetLastChangeRecord(documentId);
        
        if (!getResult.IsSuccess)
            return await historyRepository.LogAsync(documentId, accountId, currentContentHash);
        
        if (getResult.Data.ContentHash == currentContentHash)
            return Result.Success();
        
        var logResult = await historyRepository.LogAsync(documentId, accountId, currentContentHash);
        
        return logResult.IsSuccess 
            ? Result.Success()
            : Result.Failure(logResult.ErrorMessage!);
    }

    public async Task<Result> ClearChangeHistoryAsync(Guid documentId)
    {
        var deleteResult = await historyRepository.DeleteByDocumentIdAsync(documentId);
        
        return deleteResult.IsSuccess
            ? Result.Success()
            : Result.Failure(deleteResult.ErrorMessage!);
    }
    
    private string ComputeHash(string content)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

}