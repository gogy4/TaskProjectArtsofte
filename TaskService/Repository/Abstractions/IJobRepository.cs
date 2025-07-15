using TaskService.Domain;

namespace TaskService.Repository.Abstractions;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(int id);
    Task<IEnumerable<Job>> GetAllAsync(int page, int pageSize, string? filter = null);
    Task<int> AddAsync(Job job);
    Task<int> UpdateAsync(Job job);
    Task<int> DeleteAsync(int id);
    Task<int> SoftDeleteAsync(int id);
    Task<int> AssignUserAsync(int jobId, int userId);
}