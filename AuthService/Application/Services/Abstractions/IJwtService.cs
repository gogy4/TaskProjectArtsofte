namespace AuthService.Application.Helpers.Abstractions;

public interface IJwtService
{
    public string GenerateJwtToken<T>(T user);
}