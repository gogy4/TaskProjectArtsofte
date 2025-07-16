using Application.Mapper;
using Application.Models;
using Application.Services.Abstractions;
using Application.Services.Helpers;
using Infrastructure.Repository.Abstractions;
using Shared.Services;

namespace Application.Services.Implementations;

public class JobService(
    IJobRepository repository,
    HttpClient httpClient,
    IJobHistoryService service,
    IJobHelper jobHelper,
    INotificationSender notificationSender) : IJobService
{
    public async Task<IEnumerable<JobDto>> GetAllAsync(GetAllJobsRequest request)
    {
        var user = await UserHelper.GetCurrentUserId(httpClient);
        var jobs = await repository.GetAllAsync(user.Id, request.Page, request.PageSize, request.Filter);
        var jobResponses = jobs.Select(JobMapper.ToDto);
        return jobResponses;
    }

    public async Task<JobDto> GetByIdAsync(int id)
    {
        var job = await jobHelper.GetCreatedOrAssignedByUserAsync(JobMapper.ToDto(await repository.GetByIdAsync(id)),
            httpClient);
        return job;
    }

    public async Task<int> CreateAsync(JobDto jobDto)
    {
        var job = JobMapper.ToJob(jobDto);
        var user = await UserHelper.GetCurrentUserId(httpClient);
        job.CreatedAt = DateTime.UtcNow;
        job.UpdatedAt = DateTime.UtcNow;
        job.CreatedUserId = user.Id;
        var id = await repository.CreateAsync(job);
        var jobHistoryDto = new JobHistoryDto(id, "Создание таски", DateTime.UtcNow, user.Id);
        await service.CreateAsync(jobHistoryDto);
        await notificationSender.SendNotificationAsync(user.Id, $"Вы создали новую задачу: {job.Title}");
        return id;
    }

    public async Task<int> UpdateAsync(int jobId, UpdateJobRequest jobRequest)
    {
        var jobDto = await jobHelper.GetCreatedByCurrentUserAsync(await GetByIdAsync(jobId), httpClient);
        var editedJob = JobMapper.UpdateJob(jobRequest, jobDto);
        var changedCount = await repository.UpdateAsync(JobMapper.ToJob(editedJob));
        var action = jobHelper.GetDifferenceField(jobDto, editedJob);
        var jobHistoryDto = new JobHistoryDto(jobId, action, DateTime.UtcNow, jobDto.CreatedUserId);
        await service.CreateAsync(jobHistoryDto);
        await notificationSender.SendNotificationAsync(jobDto.CreatedUserId, $"Задача обновлена: {jobDto.Title}");
        if (jobDto.AssignedUserId is not null)
        {
            await notificationSender.SendNotificationAsync(jobDto.AssignedUserId.Value,
                $"Задача, назначенная вам, обновлена: {jobDto.Title}");
        }

        return jobId;
    }


    public async Task<int> SoftDeleteAsync(int id)
    {
        var jobDto = await jobHelper.GetCreatedByCurrentUserAsync(await GetByIdAsync(id), httpClient);
        var resultId = await repository.SoftDeleteAsync(id);
        var jobHistoryDto = new JobHistoryDto(resultId, "Мягкое удаление", DateTime.UtcNow, jobDto.CreatedUserId);
        await service.CreateAsync(jobHistoryDto);
        await notificationSender.SendNotificationAsync(jobDto.CreatedUserId, $"Задача мягко удалена: {jobDto.Title}");
        if (jobDto.AssignedUserId.HasValue)
        {
            await notificationSender.SendNotificationAsync(jobDto.AssignedUserId.Value,
                $"Задача, назначенная вам, мягко удалена: {jobDto.Title}");
        }

        return resultId;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var jobDto = await jobHelper.GetCreatedByCurrentUserAsync(await GetByIdAsync(id), httpClient);
        var resultId = await repository.DeleteAsync(id);
        var jobHistoryDto = new JobHistoryDto(resultId, "Полное удаление", DateTime.UtcNow, jobDto.CreatedUserId);
        await service.CreateAsync(jobHistoryDto);
        await notificationSender.SendNotificationAsync(jobDto.CreatedUserId, $"Задача удалена: {jobDto.Title}");
        if (jobDto.AssignedUserId.HasValue)
        {
            await notificationSender.SendNotificationAsync(jobDto.AssignedUserId.Value,
                $"Задача, назначенная вам, удалена: {jobDto.Title}");
        }

        return resultId;
    }

    public async Task<int> AssignUserAsync(int jobId, int userId)
    {
        var jobDto = await jobHelper.GetCreatedByCurrentUserAsync(await GetByIdAsync(jobId), httpClient);
        var isReceiverExists = await UserHelper.IsReceiverExists(httpClient, userId);
        if (!isReceiverExists) throw new ArgumentException("Получатель не найден.");
        var resultId = await repository.AssignUserAsync(jobId, userId);
        var jobHistoryDto = new JobHistoryDto(resultId, $"Назначили исполнителя с id : {userId}", DateTime.UtcNow,
            jobDto.CreatedUserId);
        await service.CreateAsync(jobHistoryDto);
        await notificationSender.SendNotificationAsync(userId, $"Вам назначена задача: {jobDto.Title}");
        await notificationSender.SendNotificationAsync(jobDto.CreatedUserId,
            $"Вы назначили задачу: {jobDto.Title} пользователю с ID {userId}");

        return resultId;
    }

    public async Task<IEnumerable<JobHistoryDto>> GetHistoryByJobIdAsync(int jobId, int pageNumber = 1,
        int pageSize = 10)
    {
        var job = await jobHelper.GetCreatedOrAssignedByUserAsync(await GetByIdAsync(jobId), httpClient);
        var history = await service.GetByJobIdAsync(jobId, pageNumber, pageSize);
        return history;
    }
}