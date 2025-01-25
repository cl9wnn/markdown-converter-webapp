using Core.Models;
using Core.Utils;

namespace Core.Interfaces.Repositories;

public interface IAccountRepository
{
     Task<Result> AddUserAsync(Account accountEntity);
     Task<Result<Account?>> GetByEmailAsync(string email);
     Task<bool> IsUserExistsByIdAsync(Guid accountId);
     Task<Result<Account>> GetByAccountIdAsync(Guid accountId);
}