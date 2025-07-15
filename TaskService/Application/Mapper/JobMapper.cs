using TaskService.Application.Dto;
using TaskService.Domain;

namespace TaskService.Application.Mapper;

public static class JobMapper
{
    public static JobDto ToResponse(Job job)
    {
        var response = new JobDto(job.Id, job.Title, job.Description, job.AssignedUserId, job.Status, job.CreatedAt, 
            job.UpdatedAt, job.IsDeleted);
        return response;
    }

    public static Job ToJob(JobDto dto)
    {
        var entity = new Job( dto.Id, dto.Title, dto.Description, dto.AssignedUserId, dto.Status,
            dto.CreateAt, dto.UpdateAt, dto.IsDeleted);
        return entity;
    }
}