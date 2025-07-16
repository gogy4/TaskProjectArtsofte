using Application.Mapper;
using Application.Models;
using Application.Services.Abstractions;
using Domain.Entity;
using Infrastructure.Repository.Abstractions;

namespace Application.Services.Implementations;

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

    public async Task<IEnumerable<JobHistoryDto>> GetByJobIdAsync(int jobId, int pageNumber, int pageSize)
    {
        return (await repository.GetByJobIdAsync(jobId, pageNumber, pageSize)).Select(JobHistoryMapper.ToDto);
    }
}