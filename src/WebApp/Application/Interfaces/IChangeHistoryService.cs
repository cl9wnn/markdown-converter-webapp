using Application.Models;
using Core.Utils;

namespace Application.Interfaces;

public interface IChangeHistoryService
{
    Task<Result<List<ChangeRecordDto>>> GetChangeHistoryAsync(Guid documentId);
    Task<Result> LogChangeAsync(Guid documentId, Guid? accountId, string content);
    Task<Result> ClearChangeHistoryAsync(Guid documentId);

}