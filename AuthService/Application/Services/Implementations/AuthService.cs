using System.Security.Claims;
using AuthService.Application.Helpers.Abstractions;
using AuthService.Application.MiddleWare;
using AuthService.Application.Services.Abstractions;

namespace AuthService.Application.Services.Implementations;

public class AuthService(
    IHttpContextAccessor httpContextAccessor,
    IJwtService jwtService,
    IBlackListService blackListService) : IAuthService
{
    public int? GetCurrentUserId()
    {
        var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
        var id = int.Parse(claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
        return id;
    }

    public void Logout(string token)
    {
        blackListService.AddTokenToBlacklist(token);
    }

    public List<string> GetCurrentUserRoles()
    {
        var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList() ?? new List<string>();
    }

    public string GenerateJwtToken<T>(T user)
    {
        return jwtService.GenerateJwtToken(user);
    }
}