using Core.Models;
using Core.Utils;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Result<string>> RegisterAsync(string email, string password, string firstName);
    Task<Result<string>> LoginAsync(string email, string password);
    Task<Result<Account>> GetAccountByEmailAsync(string email);
    Task<Result<Account>> GetAccountByIdAsync(Guid? accountId);
    Task<bool> IsExistsAsync(Guid accountId);
}