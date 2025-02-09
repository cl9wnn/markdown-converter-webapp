using Application.Interfaces;
using Application.Models;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Application.Services;

public class ChangeHistoryService(IChangeHistoryRepository historyRepository, IAccountService accountService): IChangeHistoryService
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
                DocumentId = change.DocumentId,
                UserName = accountResult.IsSuccess ? accountResult.Data.FirstName : "Unknown",
                Date = change.Date
            });
        }

        return Result<List<ChangeRecordDto>>.Success(changeRecords);
    }

    public async Task<Result> LogChangeAsync(Guid documentId, Guid? accountId)
    {
        var logResult = await historyRepository.LogAsync(documentId, accountId);
        
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
}