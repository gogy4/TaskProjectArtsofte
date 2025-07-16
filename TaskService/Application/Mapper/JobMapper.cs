using Application.Models;
using Domain.Entity;

namespace Application.Mapper;

public static class JobMapper
{
    public static JobDto ToDto(Job job)
    {
        var response = new JobDto(job.Id, job.Title, job.Description, job.Status, job.CreatedAt,
            job.UpdatedAt, job.IsDeleted, job.CreatedUserId, job.AssignedUserId);
        return response;
    }

    public static Job ToJob(JobDto dto)
    {
        var entity = new Job(dto.Id, dto.Title, dto.Description, dto.Status,
            dto.CreateAt, dto.UpdateAt, dto.IsDeleted, dto.CreatedUserId, dto.AssignedUserId);
        return entity;
    }

    public static JobDto UpdateJob(UpdateJobRequest request, JobDto job)
    {
        var newJob = new JobDto(job);
        newJob.Title = request.Title;
        newJob.Description = request.Description;
        newJob.AssignedUserId = request.AssignedUserId;
        newJob.IsDeleted = request.IsDeleted;
        newJob.Status = request.Status;
        newJob.UpdateAt = DateTime.UtcNow;
        return newJob;
    }
}