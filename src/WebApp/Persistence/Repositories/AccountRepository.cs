using Core.Interfaces;
using Core.Utils;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class AccountRepository(WebDbContext dbContext): IAccountRepository
{
    
    public async Task<Result> AddAsync(Account account)
    {
        var accountEntity = new AccountEntity
        {
            AccountId = account.AccountId,
            Email = account.Email,
            FirstName = account.FirstName,
            PasswordHash = account.PasswordHash
        };
        
        var isAccExists = await IsExistsByEmailAsync(accountEntity.Email!);

        if (isAccExists)
            return Result.Failure($"User {accountEntity.Email} not available");
        
        await dbContext.Accounts.AddAsync(accountEntity);
        await dbContext.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result<Account?>> GetByEmailAsync(string email)
    {
        var accountEntity = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Email == email);
        
        if (accountEntity == null)
            return Result<Account?>.Failure("Account with this email dont exist!");

        var account = Account.CreateAccount(accountEntity.AccountId, accountEntity.Email!, accountEntity.FirstName,  accountEntity.PasswordHash);
            
        return Result<Account>.Success(account)!;
    }

    public async Task<Result<Account>> GetByIdAsync(Guid? accountId)
    {
        var accountEntity = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.AccountId == accountId);
        
        if (accountEntity == null)
            return Result<Account?>.Failure("Account with this email dont exist!")!;

        var account = Account.CreateAccount(accountEntity.AccountId, accountEntity.Email!, accountEntity.FirstName,  accountEntity.PasswordHash);
            
        return Result<Account>.Success(account)!;
        
    }
    
    public async Task<bool> IsExistsByIdAsync(Guid accountId)
    {
        return await dbContext.Accounts.AnyAsync(a => a.AccountId == accountId);
    }
    
    private async Task<bool> IsExistsByEmailAsync(string email)
    {
        return await dbContext.Accounts.AnyAsync(a => a.Email == email);
    }
    
}