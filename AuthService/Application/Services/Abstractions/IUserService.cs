using AuthService.Application.Models;

namespace AuthService.Application.Services.Abstractions;

public interface IUserService
{
    Task<int> RegisterAsync(AuthRequest request);
    Task<string> LoginAsync(AuthRequest request);
    Task<UserModel> GetCurrentUser();
    void Logout(HttpContext httpContextAccessor);
    Task<UserModel> GetUserByIdAsync(int id);
}