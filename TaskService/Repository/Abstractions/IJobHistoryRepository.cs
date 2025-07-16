using TaskService.Application.Dto;
using TaskService.Domain;

namespace TaskService.Repository.Abstractions;

public interface IJobHistoryRepository
{
    Task<int> CreateAsync(JobHistory jobHistory);
    Task<JobHistory?> GetByIdAsync(int id);
    Task<IEnumerable<JobHistoryDto>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize);
}