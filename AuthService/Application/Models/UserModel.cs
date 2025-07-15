namespace AuthService.Application.Models;

public class UserModel
{
    public int Id { get; private set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; private set; }

    public UserModel(int id, string email, string hashedPassword, string salt)
    {
        Id = id;
        Email = email;
        HashedPassword = hashedPassword;
        Salt = salt;
    }
}