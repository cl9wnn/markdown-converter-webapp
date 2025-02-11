using Core.Models;

namespace Core.Interfaces.Services;

public interface IJwtService
{
     string GenerateJwtToken(Account account);
}