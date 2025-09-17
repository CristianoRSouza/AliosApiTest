using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ailos.Shared.Infrastructure.Security;

public class JwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;

    public JwtService(string secretKey, string issuer)
    {
        _secretKey = secretKey;
        _issuer = issuer;
    }

    public string GenerateToken(int contaId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        
        var claims = new[]
        {
            new Claim("contaId", contaId.ToString()),
            new Claim("sub", contaId.ToString()),
            new Claim("iss", _issuer),
            new Claim("aud", _issuer)
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
