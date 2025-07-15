namespace TaskService.Domain;

public class JobHistory
{
    public int Id { get; private set; }
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
}