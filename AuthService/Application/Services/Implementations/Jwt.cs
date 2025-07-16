using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class Jwt(IConfiguration configuration) : IJwt
{
    public string GenerateJwtToken<T>(T user)
    {
        var userRoles = new List<string>();

        userRoles.Add("USER");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, (user as UserModel)?.Id.ToString() ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, (user as UserModel)?.Id.ToString() ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}