namespace Domain.Entity;

public class JobHistory
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }

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