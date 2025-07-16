using TaskService.Application.Dto;
using TaskService.Domain;

namespace TaskService.Application.Mapper;

public class JobHistoryMapper
{
    public static JobHistory ToEntity(JobHistoryDto jobHistoryDto)
    {
        var entity = new JobHistory(jobHistoryDto.JobId, jobHistoryDto.Action, jobHistoryDto.Timestamp,
            jobHistoryDto.ChangedByUserId);
        return entity;
    }

    public static JobHistoryDto ToDto(JobHistory jobHistory)
    {
        var dto = new JobHistoryDto(jobHistory.Id, jobHistory.Action, DateTime.UtcNow, jobHistory.ChangedByUserId);
        return dto;
    }
}