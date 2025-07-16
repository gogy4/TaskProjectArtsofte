namespace Domain.Entity;

public class User
{
    public int Id { get; private set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; private set; }

    public User(int id, string email, string hashedPassword, string salt)
    {
        Id = id;
        Email = email;
        HashedPassword = hashedPassword;
        Salt = salt;
    }
    
    public User(string email, string hashedPassword, string salt)
    {
        Email = email;
        HashedPassword = hashedPassword;
        Salt = salt;
    }
    
}