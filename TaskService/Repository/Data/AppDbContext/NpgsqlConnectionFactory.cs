using System.Data;
using Npgsql;

namespace TaskService.Repository.Data.AppDbContext;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    public NpgsqlConnectionFactory(IConfiguration config)
    {
        connectionString = config.GetConnectionString("DefaultConnection");
    }
    
    public async Task<IDbConnection> GetConnection()
    {
        var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        return conn;
    }
}