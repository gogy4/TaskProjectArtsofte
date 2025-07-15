using TaskService.Domain;
using TaskService.Domain.Enum;

namespace TaskService.Application.Dto;

public class JobDto
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AssignedUserId { get; set; }
    public int Status { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; } 
    
    public JobDto(int id, string title, string description, int assignedUserId, int status, DateTime createAt, DateTime updateAt, bool isDeleted)
    {
        Id = id;
        Title = title;
        Description = description;
        AssignedUserId = assignedUserId;
        Status = status;
        CreateAt = createAt;
        UpdateAt = updateAt;
        IsDeleted = isDeleted;
    }
}