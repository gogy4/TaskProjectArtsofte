using TaskService.Application.Dto;
using TaskService.Application.Mapper;
using TaskService.Application.Services.Abstractions;
using TaskService.Domain;
using TaskService.Repository.Abstractions;

namespace TaskService.Application.Services.Implementations;

public class JobHistoryService(IJobHistoryRepository repository) : IJobHistoryService
{
    public async Task<int> CreateAsync(JobHistoryDto jobHistory)
    {
        var id = await repository.CreateAsync(JobHistoryMapper.ToEntity(jobHistory));
        return id;
    }

    public async Task<JobHistory> GetByIdAsync(int id)
    {
        var jobHistory = await repository.GetByIdAsync(id);
        return jobHistory;
    }

    public Task<IEnumerable<JobHistoryDto>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize)
    {
        return repository.GetByJobIdAsync(jobId, pageNumber, pageSize);
    }
}