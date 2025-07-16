public interface IEncrypt
{
    public string HashPassword(string password, string salt);
}