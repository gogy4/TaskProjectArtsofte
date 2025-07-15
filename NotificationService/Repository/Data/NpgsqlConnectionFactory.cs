using System.Data;
using Npgsql;

namespace NotificationService.Repository.Data;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> GetConnection()
    {
        var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        return conn;
    }
}