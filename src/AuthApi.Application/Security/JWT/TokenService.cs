using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Application.Security.JWT;

public class TokenService(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public string GenerateToken(string email, IEnumerable<string> roles)
    {
        var jwtPrivateKey = _config["JwtPrivateKey"];
        if (string.IsNullOrEmpty(jwtPrivateKey))
        {
            throw new InvalidOperationException("JwtPrivateKey configuration is missing or null.");
        }

        var key = Encoding.ASCII.GetBytes(jwtPrivateKey);

        var clains = new List<Claim>()
        {
            new(ClaimTypes.Email, email)
        };

        foreach (var role in roles)
        {
            clains.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(clains),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
