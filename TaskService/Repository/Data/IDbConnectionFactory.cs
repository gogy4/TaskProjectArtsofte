using System.Data;

namespace TaskService.Repository.Data.AppDbContext;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetConnection();
}