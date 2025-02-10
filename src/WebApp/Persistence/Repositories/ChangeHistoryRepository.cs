using Core.Interfaces;
using Core.Models;
using Core.Utils;
using MongoDB.Driver;
using Persistence.Entities;

namespace Persistence.Repositories;

public class ChangeHistoryRepository(IMongoDatabase database) : IChangeHistoryRepository
{
    private readonly IMongoCollection<ChangeRecordEntity> _historyCollection = database.GetCollection<ChangeRecordEntity>("ChangeHistory");
    public async Task<Result> LogAsync(Guid documentId, Guid? accountId, string contentHash)
    {
        var changeRecord = new ChangeRecordEntity
        {
            DocumentId = documentId,
            AccountId = accountId,
            ChangeDate = DateTime.UtcNow,
            ContentHash = contentHash
        };

        try
        {
            await _historyCollection.InsertOneAsync(changeRecord);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Invalid insert: {ex.Message}");
        }
    }

    public async Task<Result<List<ChangeRecord>>> GetChangesByDocumentIdAsync(Guid documentId)
    {
        var filter = Builders<ChangeRecordEntity>.Filter.Eq(record => record.DocumentId, documentId);

        var entities = await _historyCollection.Find(filter).ToListAsync();
        
        if (entities == null)
            return Result<List<ChangeRecord>>.Failure("No records found")!;

        var changeRecords = entities
            .Select(e => ChangeRecord.Create(e.DocumentId, e.AccountId, e.ChangeDate, e.ContentHash))
            .ToList();

        return Result<List<ChangeRecord>>.Success(changeRecords);
    }
    
    public async Task<Result> DeleteByDocumentIdAsync(Guid documentId)
    {
        var filter = Builders<ChangeRecordEntity>.Filter.Eq(record => record.DocumentId, documentId);
        
        try
        {
            var result = await _historyCollection.DeleteManyAsync(filter);
            
            return result.DeletedCount > 0
                ? Result.Success()
                : Result.Failure("No records found to delete");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete records: {ex.Message}");
        }
    }

    public async Task<Result<ChangeRecord>> GetLastChangeRecord(Guid documentId)
    {
        var lastLog = await _historyCollection
            .Find(x => x.DocumentId == documentId)
            .SortByDescending(x => x.ChangeDate)
            .FirstOrDefaultAsync();
        
        if (lastLog == null)
            return Result<ChangeRecord>.Failure("No records found")!;
        
        var lastChangeRecord = ChangeRecord.Create(lastLog.DocumentId, lastLog.AccountId,
            lastLog.ChangeDate, lastLog.ContentHash);

        return Result<ChangeRecord>.Success(lastChangeRecord);
    }
}