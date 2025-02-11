using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService(IOptions<AuthSettings> options): IJwtService
{
    public string GenerateJwtToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim("email", account.Email!),
            new Claim("firstname", account.FirstName!),
            new Claim("accountId", account.AccountId.ToString())
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