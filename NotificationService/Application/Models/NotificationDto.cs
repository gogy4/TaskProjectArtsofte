namespace Application.Models;

public class NotificationDto
{
    public int Id { get; private set; }
    public string Message { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public NotificationDto(int id, string message, DateTime createdAt, bool isRead, int userId)
    {
        Id = id;
        Message = message;
        CreatedAt = createdAt;
        IsRead = isRead;
        UserId = userId;
    }
}