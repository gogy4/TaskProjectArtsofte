using TaskService.Domain.Enum;

namespace TaskService.Domain;

public class Job
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AssignedUserId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } 


    public readonly List<JobHistory> History = new();

    public Job(int id, string title, string description, int assignedUserId, int status, DateTime createdAt, DateTime updatedAt, bool isDeleted)
    {
        Id = id;
        Title = title;
        Description = description;
        AssignedUserId = assignedUserId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsDeleted = isDeleted;
    }
}