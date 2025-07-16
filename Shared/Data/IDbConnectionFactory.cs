using System.Data;

namespace Shared.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnection();
}