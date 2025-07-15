namespace AuthService.Application.Services.Abstractions;

public interface IAuthService
{
    int? GetCurrentUserId();
    void Logout(string token);
    List<string> GetCurrentUserRoles();
    public string GenerateJwtToken<T>(T user);

}