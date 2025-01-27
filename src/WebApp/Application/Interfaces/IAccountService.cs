using Core.Utils;

namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result<string>> RegisterAsync(string email, string password, string firstName);
    Task<Result<string>> LoginAsync(string email, string password);
    Task<Result<Guid>> GetAccountIdByEmailAsync(string email);
    Task<bool> DoesUserExistAsync(Guid accountId);
}