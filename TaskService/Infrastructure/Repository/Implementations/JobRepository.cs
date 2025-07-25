﻿using Dapper;
using Domain.Entity;
using Infrastructure.Repository.Abstractions;
using Shared.Data;

namespace Infrastructure.Repository.Implementations;

public class JobRepository(IDbConnectionFactory factory) : IJobRepository
{
    public async Task<Job?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT * FROM ""Jobs"" WHERE ""Id"" = @Id";
        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<Job>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Job>> GetAllAsync(int userId, int page, int pageSize, string? filter = null)
    {
        var sql = @"
        SELECT * FROM ""Jobs""
        WHERE ""IsDeleted"" = FALSE
          AND (""CreatedUserId"" = @UserId OR ""AssignedUserId"" = @UserId)
    ";

        if (!string.IsNullOrEmpty(filter))
            sql += " AND \"Title\" ILIKE @Filter";

        sql += " ORDER BY \"CreatedAt\" DESC OFFSET @Offset LIMIT @Limit";

        var parameters = new
        {
            UserId = userId,
            Filter = $"%{filter}%",
            Offset = (page - 1) * pageSize,
            Limit = pageSize
        };

        using var connection = await factory.GetConnection();
        return await connection.QueryAsync<Job>(sql, parameters);
    }


    public async Task<int> CreateAsync(Job job)
    {
        const string sql = @"
            INSERT INTO ""Jobs"" 
                (""Title"", ""Description"", ""AssignedUserId"", ""Status"", ""CreatedAt"", ""UpdatedAt"", ""IsDeleted"", ""CreatedUserId"")
            VALUES 
                (@Title, @Description, @AssignedUserId, @Status, @CreatedAt, @UpdatedAt, @IsDeleted, @CreatedUserId)
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
                ""UpdatedAt"" = @UpdatedAt,
                ""IsDeleted"" = @IsDeleted
                WHERE ""Id"" = @Id";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteAsync(sql, job);
    }

    public async Task<int> DeleteAsync(int id)
    {
        const string sql = @"
        DELETE FROM ""Jobs""
        WHERE ""Id"" = @Id
        RETURNING ""Id"";";

        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<int>(sql, new { Id = id });
    }

    public async Task<int> SoftDeleteAsync(int id)
    {
        const string sql = @"
        UPDATE ""Jobs""
        SET ""IsDeleted"" = TRUE
        WHERE ""Id"" = @Id
        RETURNING ""Id"";";

        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<int>(sql, new { Id = id });
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