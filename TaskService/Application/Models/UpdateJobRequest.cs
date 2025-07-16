using System.Text.Json.Serialization;
using Domain.Enum;

namespace Application.Models;

public class UpdateJobRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? AssignedUserId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public JobStatus Status { get; set; }
    
    [JsonIgnore]
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; }

    public UpdateJobRequest(int id, string title, string description, JobStatus status,
        DateTime updateAt, bool isDeleted, int? assignedUserId = null)
    {
        Id = id;
        Title = title;
        Description = description;
        AssignedUserId = assignedUserId;
        Status = status;
        UpdateAt = updateAt;
        IsDeleted = isDeleted;
    }
}