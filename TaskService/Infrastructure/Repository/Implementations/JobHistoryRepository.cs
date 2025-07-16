using Dapper;
using Domain.Entity;
using Infrastructure.Repository.Abstractions;
using Shared.Data;

namespace Infrastructure.Repository.Implementations;

public class JobHistoryRepository(IDbConnectionFactory factory) : IJobHistoryRepository
{
    public async Task<int> CreateAsync(JobHistory jobHistory)
    {
        const string sql = @"
           insert into ""JobHistory"" (""JobId"", ""Action"", ""Timestamp"", ""ChangedByUserId"")
           values (@JobId, @Action, @TimeStamp, @ChangedByUserId)
           returning ""Id"";";
        using var connection = await factory.GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, jobHistory);
    }

    public async Task<JobHistory?> GetByIdAsync(int id)
    {
        const string sql = @"select * from ""JobHistory"" where ""JobId"" = @id";
        using var connection = await factory.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<JobHistory>(sql, new { id });
    }

    public async Task<IEnumerable<JobHistory>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize)
    {
        const string sql = @"
        SELECT * 
        FROM ""JobHistory"" 
        WHERE ""JobId"" = @jobId
        ORDER BY ""Timestamp"" DESC
        OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

        using var connection = await factory.GetConnection();

        var parameters = new
        {
            jobId,
            offset = (pageNumber - 1) * pageSize,
            limit = pageSize
        };

        var result = await connection.QueryAsync<JobHistory>(sql, parameters);
        return result;
    }
}