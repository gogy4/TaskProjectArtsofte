using Domain.Entity;

namespace Infrastructure.Repository.Abstractions;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(int id);
    Task<IEnumerable<Job>> GetAllAsync(int userId, int page, int pageSize, string? filter = null);
    Task<int> CreateAsync(Job job);
    Task<int> UpdateAsync(Job job);
    Task<int> DeleteAsync(int id);
    Task<int> SoftDeleteAsync(int id);
    Task<int> AssignUserAsync(int jobId, int userId);
}