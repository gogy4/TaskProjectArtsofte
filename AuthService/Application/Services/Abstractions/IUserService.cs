using AuthService.Application.Models;


public interface IUserService
{
    Task<int> RegisterAsync(AuthRequest request);
    Task<string> LoginAsync(AuthRequest request);
    Task<UserModel> GetCurrentUser();
    void Logout(HttpContext httpContextAccessor);
    Task<UserModel> GetUserByIdAsync(int id);
}