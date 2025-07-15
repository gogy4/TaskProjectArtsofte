
public interface IAuth
{
    int? GetCurrentUserId();
    void Logout(string token);
    List<string> GetCurrentUserRoles();
    public string GenerateJwtToken<T>(T user);

}