using Core.interfaces;
using Core.Models;
using Core.Utils;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AccountService(IAccountRepository accountRepository, JwtService jwtService)
{
    public async Task<Result<string>> RegisterAsync(string email, string password, string firstName)
    {
        var account = new Account
        {
            AccountId = Guid.NewGuid(),
            Email = email,
            PasswordHash = password,
            FirstName = firstName
        };
        
        var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
        account.PasswordHash = passwordHash;
        
        var addResult = await accountRepository.AddUserAsync(account);

        if (addResult.IsSuccess)
        {
            var token = jwtService.GenerateJwtToken(account!);
            return Result<string>.Success(token);
        }
        return Result<string>.Failure(addResult.ErrorMessage!)!;
    }

    public async Task<Result<string>?> LoginAsync(string email, string password)
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

    public async Task<Result<Guid>> GetAccountIdByEmail(string email)
    {
        var accountResult = await accountRepository.GetByEmailAsync(email);
        
        return accountResult.IsSuccess
            ? Result<Guid>.Success(accountResult.Data!.AccountId)
            : Result<Guid>.Failure(accountResult.ErrorMessage!)!;
    }

    public async Task<bool> DoesUserExistAsync(Guid accountId)
    { 
        return await accountRepository.IsUserExistsById(accountId);
    }
}