using Domain.Entity;

namespace Infrastructure.Repository.Abstractions;

public interface IJobHistoryRepository
{
    Task<int> CreateAsync(JobHistory jobHistory);
    Task<JobHistory?> GetByIdAsync(int id);
    Task<IEnumerable<JobHistory>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize);
}