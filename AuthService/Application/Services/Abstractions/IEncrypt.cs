namespace AuthService.Application.Services.Abstractions;

public interface IEncrypt
{
    public string HashPassword(string password, string salt);
}