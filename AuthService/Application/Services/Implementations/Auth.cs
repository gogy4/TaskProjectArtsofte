using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class Auth(
    IHttpContextAccessor httpContextAccessor,
    IJwt jwt,
    IBlackListService blackListService) : IAuth
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
        return jwt.GenerateJwtToken(user);
    }
}