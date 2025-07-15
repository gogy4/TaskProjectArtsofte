using System.Data;

namespace NotificationService.Repository.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnection();
}