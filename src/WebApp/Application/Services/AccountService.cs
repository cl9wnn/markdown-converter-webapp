using Core.interfaces;
using Core.Models;
using Core.Utils;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AccountService(IAccountRepository accountRepository, JwtService jwtService)
{
    public async Task<Result> RegisterAsync(string email, string password, string firstName)
    {
        var account = new Account
        {
            Email = email,
            PasswordHash = password,
            FirstName = firstName
        };
        
        var passwordHash = new PasswordHasher<Account>().HashPassword(account, password);
        account.PasswordHash = passwordHash;
        
        var addResult = await accountRepository.AddUserAsync(account);

        return addResult.IsSuccess
            ? Result.Success()
            : Result.Failure(addResult.ErrorMessage!);
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
}