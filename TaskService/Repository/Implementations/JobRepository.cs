using Dapper;
using TaskService.Domain;
using TaskService.Repository.Abstractions;
using TaskService.Repository.Data.AppDbContext;

namespace TaskService.Repository.Implementations;

public class JobRepository(IDbConnectionFactory factory) : IJobRepository
{
    public async Task<Job?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT * FROM ""Jobs"" WHERE ""Id"" = @Id AND ""IsDeleted"" = FALSE";
        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<Job>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Job>> GetAllAsync(int page, int pageSize, string? filter = null)
    {
        var sql = @"SELECT * FROM ""Jobs"" WHERE ""IsDeleted"" = FALSE";
        if (!string.IsNullOrEmpty(filter))
            sql += " AND \"Title\" ILIKE @Filter";

        sql += " ORDER BY \"CreatedAt\" DESC OFFSET @Offset LIMIT @Limit";

        var parameters = new
        {
            Filter = $"%{filter}%",
            Offset = (page - 1) * pageSize,
            Limit = pageSize
        };

        using var connection = await factory.GetConnection();
        return await connection.QueryAsync<Job>(sql, parameters);
    }

    public async Task<int> AddAsync(Job job)
    {
        const string sql = @"
            INSERT INTO ""Jobs"" 
                (""Title"", ""Description"", ""AssignedUserId"", ""Status"", ""CreatedAt"", ""UpdatedAt"", ""IsDeleted"")
            VALUES 
                (@Title, @Description, @AssignedUserId, @Status, @CreatedAt, @UpdatedAt, FALSE)
            RETURNING ""Id"";";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, job);
    }

    public async Task<int> UpdateAsync(Job job)
    {
        const string sql = @"
            UPDATE ""Jobs"" SET
                ""Title"" = @Title,
                ""Description"" = @Description,
                ""AssignedUserId"" = @AssignedUserId,
                ""Status"" = @Status,
                ""UpdatedAt"" = @UpdatedAt
            WHERE ""Id"" = @Id";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteAsync(sql, job);
    }

    public async Task<int> DeleteAsync(int id)
    {
        const string sql = @"DELETE FROM ""Jobs"" WHERE ""Id"" = @Id";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<int> SoftDeleteAsync(int id)
    {
        const string sql = @"UPDATE ""Jobs"" SET ""IsDeleted"" = TRUE WHERE ""Id"" = @Id";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<int> AssignUserAsync(int jobId, int userId)
    {
        const string sql = @"
            UPDATE ""Jobs"" SET 
                ""AssignedUserId"" = @UserId, 
                ""UpdatedAt"" = NOW() 
            WHERE ""Id"" = @JobId AND ""IsDeleted"" = FALSE";

        using var connection = await factory.GetConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId, JobId = jobId });
        return jobId; 
    }
}
