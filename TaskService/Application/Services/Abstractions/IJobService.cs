using TaskService.Application.Dto;
using TaskService.Domain;

namespace TaskService.Application.Services.Abstractions;

public interface IJobService
{
    Task<IEnumerable<JobDto>> GetAllAsync(GetAllJobsRequest request);
    Task<JobDto> GetByIdAsync(int id);
    Task<int> CreateAsync(JobDto jobDto);
    Task<int> UpdateAsync(JobDto jobDto);
    Task<int> SoftDeleteAsync(int id);
    Task<int> DeleteAsync(int id);
    Task<int> AssignUserAsync(int jobId, int userId);
}