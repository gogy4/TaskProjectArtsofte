using Dapper;
using Domain.Entity;
using Infrastructure.Repository.Abstractions;
using Shared.Data;

namespace Infrastructure.Repository.Implementations;

public class NotificationRepository(IDbConnectionFactory factory) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT * FROM ""Notifications""
            WHERE ""UserId"" = @UserId
            ORDER BY ""CreatedAt"" DESC;";

        using var connection = await factory.GetConnection();
        return await connection.QueryAsync<Notification>(sql, new { UserId = userId });
    }

    public async Task<Notification> GetByIdAsync(int id)
    {
        const string sql = @"
            select * from ""Notifications""
            where ""Id"" = @Id";

        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<Notification>(sql, new { Id = id });
    }

    public async Task<int> AddAsync(Notification notification)
    {
        const string sql = @"
            INSERT INTO ""Notifications""
                (""UserId"", ""Message"", ""CreatedAt"", ""IsRead"")
            VALUES 
                (@UserId, @Message, @CreatedAt, @IsRead)
            RETURNING ""Id"";";

        using var connection = await factory.GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, notification);
    }

    public async Task MarkAsReadAsync(int id)
    {
        const string sql = @"
            UPDATE ""Notifications""
            SET ""IsRead"" = TRUE
            WHERE ""Id"" = @Id;";

        using var connection = await factory.GetConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}