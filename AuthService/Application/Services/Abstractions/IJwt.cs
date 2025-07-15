
public interface IJwt
{
    public string GenerateJwtToken<T>(T user);
}