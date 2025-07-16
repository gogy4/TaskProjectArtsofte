using System.Text.Json.Serialization;
using TaskService.Domain;
using TaskService.Domain.Enum;

namespace TaskService.Application.Dto;

public class JobDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? AssignedUserId { get; set; } = null;
    public int Status { get; set; }
    
    [JsonIgnore]
    public DateTime CreateAt { get; set; }
    
    [JsonIgnore]
    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    [JsonIgnore]
    public int CreatedUserId { get; set; }

    public JobDto(int id, string title, string description, int status, DateTime createAt,
        DateTime updateAt, bool isDeleted, int createdUserId, int? assignedUserId = null)
    {
        Id = id;
        Title = title;
        Description = description;
        AssignedUserId = assignedUserId;
        Status = status;
        CreateAt = createAt;
        UpdateAt = updateAt;
        IsDeleted = isDeleted;
        CreatedUserId = createdUserId;
    }

    public JobDto(JobDto job)
    {
        Id = job.Id;
        Title = job.Title;
        Description = job.Description;
        AssignedUserId = job.AssignedUserId;
        Status = job.Status;
        CreateAt = job.CreateAt;
        UpdateAt = job.UpdateAt;
        IsDeleted = job.IsDeleted;
        CreatedUserId = job.CreatedUserId;
    }
}