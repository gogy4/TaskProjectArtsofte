using Application.Models;
using Domain.Entity;

namespace Application.Services.Abstractions;

public interface IJobHistoryService
{
    Task<int> CreateAsync(JobHistoryDto jobHistory);
    Task<JobHistory> GetByIdAsync(int id);
    Task<IEnumerable<JobHistoryDto>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize);
}