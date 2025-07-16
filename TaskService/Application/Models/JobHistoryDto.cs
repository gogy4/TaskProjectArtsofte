using System.Text.Json.Serialization;

namespace TaskService.Application.Dto;

public class JobHistoryDto
{
    [JsonIgnore]
    public int Id { get; set; }
    
    public int JobId { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public int ChangedByUserId{get;set;}

    public JobHistoryDto(int id, int jobId, string action, DateTime timestamp, int changedByUserId)
    {
        Id = id;
        Action = action;
        Timestamp = timestamp;
        ChangedByUserId = changedByUserId;
        JobId = jobId;
    }

    public JobHistoryDto(int jobId, string action, DateTime timestamp, int changedByUserId)
    {
        Action = action;
        Timestamp = timestamp;
        ChangedByUserId = changedByUserId;
        JobId = jobId;
    }
}