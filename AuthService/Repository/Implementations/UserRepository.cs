using AuthService.Domain.Entity;
using AuthService.Repository.Abstractions;
using AuthService.Repository.AppDbContext;
using Dapper;

namespace AuthService.Repository.Implementations;

public class UserRepository(IDbConnectionFactory factory) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email";
        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT * FROM ""Users"" WHERE ""Id"" = @Id";
        using var connection = await factory.GetConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO ""Users"" (""Email"", ""HashedPassword"", ""Salt"")
            VALUES (@Email, @HashedPassword, @Salt)
            RETURNING ""Id"";
        ";
        using var connection = await factory.GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, user);
    }
}