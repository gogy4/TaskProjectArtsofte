using TaskService.Application.Dto;
using TaskService.Application.Mapper;
using TaskService.Application.Services.Abstractions;
using TaskService.Domain;
using TaskService.Repository.Abstractions;

namespace TaskService.Application.Services.Implementations;

public class JobService(IJobRepository repository) : IJobService
{
    public async Task<IEnumerable<JobDto>> GetAllAsync(GetAllJobsRequest request)
    {
        var jobs = await repository.GetAllAsync(request.Page, request.PageSize, request.Filter);
        var jobResponses = jobs.Select(JobMapper.ToResponse);
        return jobResponses;
    }

    public async Task<JobDto> GetByIdAsync(int id)
    {
        var job = await repository.GetByIdAsync(id);
        var jobResponses = JobMapper.ToResponse(job);
        return jobResponses;
    }

    public async Task<int> CreateAsync(JobDto jobDto)
    {
        var job = JobMapper.ToJob(jobDto);
        var id = await repository.AddAsync(job);
        return id;
    }

    public async Task<int> UpdateAsync(JobDto jobDto)
    {
        var job = JobMapper.ToJob(jobDto);
        var id = await repository.UpdateAsync(job);
        return id;
    }

    public async Task<int> SoftDeleteAsync(int id)
    {
        var resultId = await repository.SoftDeleteAsync(id);
        return resultId;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var resultId = await repository.DeleteAsync(id);
        return resultId;
    }

    public async Task<int> AssignUserAsync(int jobId, int userId)
    {
        var resultId = await repository.AssignUserAsync(jobId, userId);
        return resultId;
    }
}