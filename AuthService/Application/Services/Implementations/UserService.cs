using AuthService.Application.Mapper;
using AuthService.Application.Models;
using AuthService.Domain.Entity;
using AuthService.Repository.Abstractions;

namespace AuthService.Application.Services.Implementations;

public class UserService(IUserRepository repository, IEncrypt encrypt, IAuth auth) : IUserService
{
    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        var user = await repository.GetByIdAsync(id);
        return UserMapper.ToModel(user);
    }
    public async Task<int> RegisterAsync(AuthRequest request)
    {
        //TODO доделать валидацию
        var salt = Guid.NewGuid().ToString();
        var user = new User(request.Email, encrypt.HashPassword(request.Password, salt), salt);
        var id = await repository.CreateAsync(user);
        return id;
    }

    public async Task<string> LoginAsync(AuthRequest request)
    {
        if (auth.GetCurrentUserId() != -1) return "Вы уже авторизованы";
        var userModel = await ValidateCredentials(request.Email, request.Password);
        var token = auth.GenerateJwtToken(userModel);
        return token;
    }

    public async Task<UserModel> GetCurrentUser()
    {
        var id = auth.GetCurrentUserId();
        if (id is null or -1) throw new ArgumentException("Пользователь не авторизован");
        var user = await repository.GetByIdAsync((int)id);
        return UserMapper.ToModel(user);
    }

    public void Logout(HttpContext httpContextAccessor)
    {
        var token = httpContextAccessor.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Пустой токен");
        }

        auth.Logout(token);
    }

    private async Task<UserModel?> ValidateCredentials(string email, string password)
    {
        var user = await repository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new ArgumentNullException("Почта пользователя не найдена");
        }

        var hashedPassword = encrypt.HashPassword(password, user.Salt);

        if (user.HashedPassword != hashedPassword)
        {
            throw new ArgumentException("Пароли не совпадают");
        }

        return UserMapper.ToModel(user);
    }
    
}