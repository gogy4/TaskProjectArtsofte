using System.Data;

namespace AuthService.Repository.AppDbContext;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnection();
}