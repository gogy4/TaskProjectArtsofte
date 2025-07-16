using System.Text.Json.Serialization;

namespace TaskService.Domain;

public class JobHistory
{
    [JsonIgnore]
    public int Id { get; set; }
    public int JobId { get; set; }
    public string Action { get; set; }
    [JsonIgnore]
    public DateTime Timestamp { get; set; }
    
    [JsonIgnore]
    public int ChangedByUserId { get; set; }

    public JobHistory(int jobId, string action, DateTime timestamp, int changedByUserId)
    {
        JobId = jobId;
        Action = action;
        Timestamp = timestamp;
        ChangedByUserId = changedByUserId;
    }
    
    public JobHistory(int id, int jobId, string action, DateTime timestamp, int changedByUserId)
    {
        Id = id;
        JobId = jobId;
        Action = action;
        Timestamp = timestamp;
        ChangedByUserId = changedByUserId;
    }
}