using Core.Models;
using Core.Utils;

namespace Core.Interfaces;

public interface IAccountRepository
{
     Task<Result> AddAsync(Account accountEntity);
     Task<Result<Account?>> GetByEmailAsync(string email);
     Task<bool> IsExistsByIdAsync(Guid accountId);
     Task<Result<Account>> GetByIdAsync(Guid? accountId);
}