using Application.Interfaces;
using Core.Interfaces;
using Core.Models;
using Core.Utils;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AccountService(IAccountRepository accountRepository, JwtService jwtService): IAccountService
{
    public async Task<Result<string>> RegisterAsync(string email, string password, string firstName)
    {
        var account = Account.CreateAccount(Guid.NewGuid(), email, firstName, password);
        
        var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
        
        account.PasswordHash = passwordHash;
        
        var addResult = await accountRepository.AddAsync(account);

        if (addResult.IsSuccess)
        {
            var token = jwtService.GenerateJwtToken(account!);
            return Result<string>.Success(token);
        }
        return Result<string>.Failure(addResult.ErrorMessage!)!;
    }

    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        var accountResult = await accountRepository.GetByEmailAsync(email);
        
        if (!accountResult.IsSuccess)
            return Result<string>.Failure(accountResult.ErrorMessage!)!;

        var account = accountResult.Data;
        
        var verifyResult = new PasswordHasher<Account>().VerifyHashedPassword(account!, account!.PasswordHash!, password);

        if (verifyResult == PasswordVerificationResult.Success)
        {
            var token = jwtService.GenerateJwtToken(account!);
            return Result<string>.Success(token);
        }

        return Result<string>.Failure("Invalid password!"!)!;
    }

    public async Task<Result<Account>> GetAccountByEmailAsync(string email)
    {
        var accountResult = await accountRepository.GetByEmailAsync(email);
        
        return accountResult.IsSuccess
            ? Result<Account>.Success(accountResult.Data!)
            : Result<Account>.Failure(accountResult.ErrorMessage!)!;
    }

    public async Task<Result<Account>> GetAccountByIdAsync(Guid? accountId)
    {
        var accountResult = await accountRepository.GetByIdAsync(accountId);
        
        return accountResult.IsSuccess
            ? Result<Account>.Success(accountResult.Data)
            : Result<Account>.Failure(accountResult.ErrorMessage!)!;
    }

    public async Task<bool> IsExistsAsync(Guid accountId)
    { 
        return await accountRepository.IsExistsByIdAsync(accountId);
    }
}