using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;

namespace BusinessLogic.Services;

public class JwtService(IOptions<AuthSettings> options)
{
    public string GenerateJwtToken(Account accountEntity)
    {
        var claims = new List<Claim>
        {
            new Claim("email", accountEntity.Email!),
            new Claim("firstname", accountEntity.FirstName!),
            new Claim("id", accountEntity.AccountId.ToString())
        };
        
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                options.Value.SecretKey!)), SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}